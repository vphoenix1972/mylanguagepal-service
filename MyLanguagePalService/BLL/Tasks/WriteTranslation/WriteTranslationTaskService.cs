using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Extensions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public sealed class WriteTranslationTaskService : QuizTaskServiceBase<QuizTaskSettings, QuizTaskRunModel, QuizTaskAnswersModel, QuizTaskSummary>
    {
        public const int MinCountOfWordsUsed = 1;
        public const int MaxCountOfWordsUsed = 1000;
        public const int DefaultCountOfWordsUsed = 30;

        public WriteTranslationTaskService(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db, int taskId, [NotNull] string name) :
            base(phrasesService, languagesService, db, taskId, name)
        {
        }

        protected override QuizTaskSettings SetSettingsImpl(QuizTaskSettings settings)
        {
            Assert(settings);

            return base.SetSettingsImpl(settings);
        }

        protected override QuizTaskRunModel RunNewTaskImpl(QuizTaskSettings settings)
        {
            Assert(settings);

            /* Logic */
            var currentDate = DateTime.UtcNow;

            var phrases = Db.Phrases
                .Where(p => p.LanguageId == settings.LanguageId)
                .ToList();

            var phrasesToRepeat = new List<PhraseDal>();
            foreach (var phrase in phrases)
            {
                var knowledgeLevel = phrase.KnowledgeLevels.SingleOrDefault(l => l.TaskId == TaskId);
                if (knowledgeLevel == null)
                {
                    // Phrase has been used in task yet
                    phrasesToRepeat.Add(phrase);
                    if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                        break; // Limit reached
                }
                else
                {
                    if (knowledgeLevel.CurrentLevel <= (currentDate - knowledgeLevel.LastRepetitonTime).Days)
                    {
                        // It is time to repeat the phrase
                        phrasesToRepeat.Add(phrase);
                        if (phrasesToRepeat.Count >= settings.CountOfWordsUsed)
                            break; // Limit reached
                    }

                    // Skip phrase - knowledge level is ok
                }
            }

            var result = new QuizTaskRunModel()
            {
                Phrases = phrasesToRepeat.Select(p => new Phrase(p)).ToList()
            };

            return result;
        }

        protected override QuizTaskSummary FinishTaskImpl(QuizTaskSettings settings, QuizTaskAnswersModel result)
        {
            /* Validation */
            Assert(settings);
            Assert(result);

            /* Check answers */
            var checkedAnswers = new List<QuizTaskResult<Phrase>>();
            for (var i = 0; i < result.Answers.Count; i++)
            {
                var answer = result.Answers[i];
                if (answer == null)
                    throw new ArgumentNullException($"{result.Answers}[{i}]");

                var phrase = PhrasesService.GetPhrase(answer.PhraseId);
                if (phrase == null)
                    throw new ArgumentException($"Phrase in {result.Answers}[{i}] does not exist");

                if (phrase.LanguageId != settings.LanguageId)
                    throw new ArgumentException($"Phrase in {result.Answers}[{i}] has different language from language in settings.");

                var isCorrect = answer.Answers.Count > 0;
                foreach (var input in answer.Answers)
                {
                    isCorrect &= PhrasesService.GetTranslations(phrase).Any(ts => ts.Phrase.Text == input);
                    if (!isCorrect)
                        break;
                }

                checkedAnswers.Add(new QuizTaskResult<Phrase>()
                {
                    Entity = new Phrase(phrase),
                    IsCorrect = isCorrect
                });
            }

            /* Update knowledge levels */
            foreach (var answer in checkedAnswers)
            {
                Db.AddOrUpdate(
                    dbSetGetter: db => db.KnowledgeLevels,
                    searcher: dbSet => dbSet.FirstOrDefault(l => l.TaskId == TaskId && l.PhraseId == answer.Entity.Id),
                    setter: l =>
                    {
                        l.PhraseId = answer.Entity.Id;
                        l.TaskId = TaskId;
                        l.LastRepetitonTime = DateTime.UtcNow;

                        var previousLevel = l.CurrentLevel;

                        if (answer.IsCorrect)
                        {
                            if (l.CurrentLevel >= 1)
                            {
                                l.CurrentLevel *= 2 + GetCoef(l.PreviousLevel);
                            }
                            else
                            {
                                l.CurrentLevel = 1;
                            }

                            if (l.CurrentLevel > 30)
                                l.CurrentLevel = 30; // MAX                            

                        }
                        else
                        {
                            l.PreviousLevel = l.CurrentLevel;
                            l.CurrentLevel = 0;
                        }

                        answer.Improvement = l.CurrentLevel - previousLevel;
                    }
                );
            }

            return new QuizTaskSummary()
            {
                Results = checkedAnswers
            };
        }

        private int GetCoef(int? previousLevel)
        {
            if (!previousLevel.HasValue)
                return 0;

            if (previousLevel.Value < 4)
                return 0;

            if (previousLevel.Value >= 4)
                return 1;

            return 2;
        }

        protected override QuizTaskSettings DefaultSettings()
        {
            return DefaultSettings(DefaultCountOfWordsUsed);
        }

        private void Assert(QuizTaskSettings settings)
        {
            Assert(settings, MinCountOfWordsUsed, MaxCountOfWordsUsed);
        }
    }

    //public class WriteTranslationTaskService : ServiceBase,
    //                                           IWriteTranslationTaskService
    //{
    //    public const int MinCountOfWordsUsed = 1;
    //    public const int MaxCountOfWordsUsed = 1000;
    //    public const int DefaultCountOfWordsUsed = 30;

    //    private readonly IPhrasesService _phrasesService;
    //    private readonly ILanguagesService _languagesService;
    //    private readonly IApplicationDbContext _db;

    //    public WriteTranslationTaskService(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db)
    //    {
    //        _phrasesService = phrasesService;
    //        _languagesService = languagesService;
    //        _db = db;
    //    }

    //    public WriteTranslationTaskSettingModel GetSettings()
    //    {
    //        var settingDal = _db.WriteTranslationTaskSettings.FirstOrDefault();
    //        if (settingDal == null)
    //        {
    //            // Return default one
    //            return new WriteTranslationTaskSettingModel()
    //            {
    //                LanguageId = _languagesService.GetDefaultLanguage().Id,
    //                CountOfWordsUsed = DefaultCountOfWordsUsed
    //            };
    //        }

    //        // Return default one
    //        return new WriteTranslationTaskSettingModel()
    //        {
    //            LanguageId = settingDal.LanguageId,
    //            CountOfWordsUsed = settingDal.CountOfWordsUsed
    //        };
    //    }

    //    public void SetSettings(WriteTranslationTaskSettingModel settings)
    //    {
    //        /* Validate */
    //        Assert(settings);

    //        /* Save */
    //        var settingDal = _db.WriteTranslationTaskSettings.FirstOrDefault();
    //        var isNew = settingDal == null;
    //        if (isNew)
    //        {
    //            settingDal = new WriteTranslationTaskSettingDal();
    //        }

    //        settingDal.LanguageId = settings.LanguageId;
    //        settingDal.CountOfWordsUsed = settings.CountOfWordsUsed;

    //        if (isNew)
    //        {
    //            _db.WriteTranslationTaskSettings.Add(settingDal);
    //        }
    //        else
    //        {
    //            _db.MarkModified(settingDal);
    //        }
    //    }

    //    public WriteTranslationTaskRunModel RunNewTask(WriteTranslationTaskSettingModel settings)
    //    {
    //        /* Validation */
    //        Assert(settings);

    //        /* Logic */
    //        var phrases = _db.Phrases
    //            .Where(p => p.LanguageId == settings.LanguageId)
    //            .OrderBy(
    //                p => p.WriteTranslationTaskJournalRecords.Sum(r => (int?)(r.CorrectWrongAnswersDelta)) ?? 0
    //            )
    //            .Take(settings.CountOfWordsUsed)
    //            .ToList();

    //        var result = new WriteTranslationTaskRunModel()
    //        {
    //            Phrases = phrases.Select(p => new PhraseWithTranslations(p)
    //            {
    //                Translations = _phrasesService.GetTranslations(p.Id)
    //            }).ToList()
    //        };

    //        return result;
    //    }

    //    public void FinishTask(WriteTranslationTaskFinishedSummaryModel summary)
    //    {
    //        /* Validation */
    //        Assert(summary);

    //        /* Logic */
    //        foreach (var record in summary.Results)
    //        {
    //            var delta = record.CorrectAnswersCount - record.WrongAnswersCount;

    //            var recordDal = _db.WriteTranslationTaskJournal.FirstOrDefault(e => e.PhraseId == record.PhraseId);
    //            var isNew = recordDal == null;
    //            if (isNew)
    //            {
    //                recordDal = new WriteTranslationTaskJournalRecordDal();
    //            }

    //            recordDal.PhraseId = record.PhraseId;
    //            recordDal.LastRepetitonTime = DateTime.UtcNow;
    //            recordDal.CorrectWrongAnswersDelta += delta;

    //            if (isNew)
    //            {
    //                _db.WriteTranslationTaskJournal.Add(recordDal);
    //            }
    //            else
    //            {
    //                _db.MarkModified(recordDal);
    //            }
    //        }
    //    }

    //    private void Assert(WriteTranslationTaskSettingModel settings)
    //    {
    //        if (settings == null)
    //            throw new ArgumentNullException(nameof(settings));

    //        if (!_languagesService.CheckIfLanguageExists(settings.LanguageId))
    //            throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

    //        if (settings.CountOfWordsUsed < MinCountOfWordsUsed || settings.CountOfWordsUsed > MaxCountOfWordsUsed)
    //        {
    //            throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
    //                $"Count of words used must be between {MinCountOfWordsUsed} and {MaxCountOfWordsUsed} words");
    //        }
    //    }

    //    private void Assert(WriteTranslationTaskFinishedSummaryModel summary)
    //    {
    //        if (summary == null)
    //            throw new ArgumentNullException(nameof(summary));

    //        if (summary.Results == null)
    //            throw new ArgumentNullException(nameof(summary.Results));

    //        for (var i = 0; i < summary.Results.Count; i++)
    //        {
    //            var record = summary.Results[i];

    //            if (record == null)
    //                throw new ArgumentNullException($"Results[{i}]");

    //            var phraseId = record.PhraseId;

    //            if (!_phrasesService.CheckIfPhraseExists(phraseId))
    //                throw new ValidationFailedException($"Results[{i}]", $"Phrase with id '{phraseId}' does not exists");

    //            string message;
    //            if (!ValidateAnswersCount(record.CorrectAnswersCount, out message))
    //                throw new ValidationFailedException($"Results[{i}].{nameof(record.CorrectAnswersCount)}", message);
    //            if (!ValidateAnswersCount(record.WrongAnswersCount, out message))
    //                throw new ValidationFailedException($"Results[{i}].{nameof(record.WrongAnswersCount)}", message);
    //        }
    //    }

    //    private bool ValidateAnswersCount(int answersCount, out string message)
    //    {
    //        message = string.Empty;

    //        if (answersCount < 0)
    //        {
    //            message = "Answers count cannot be less than 0";
    //            return false;
    //        }

    //        return true;
    //    }
    //}
}