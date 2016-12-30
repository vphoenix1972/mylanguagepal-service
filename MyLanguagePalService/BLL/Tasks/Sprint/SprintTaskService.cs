using System;
using System.Collections.Generic;
using System.Linq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.Core;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public sealed class SprintTaskService : QuizTaskServiceBase<SprintTaskSettings, QuizTaskRunModel, IList<SprintTaskAnswer>, QuizTaskSummary>
    {
        public const int SprintTaskId = 3;
        public const string SprintTaskName = "sprint";
        public const int MinTotalTimeForTask = 5;
        public const int DefaultTotalTimeForTask = 60;

        public SprintTaskService(IFramework framework, IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db) :
            base(framework, phrasesService, languagesService, db)
        {
        }

        public override string Name => SprintTaskName;

        protected override int TaskId => SprintTaskId;

        protected override SprintTaskSettings DefaultSettings()
        {
            var result = base.DefaultSettings();

            result.TotalTimeForTask = DefaultTotalTimeForTask;

            return result;
        }

        protected override void Assert(SprintTaskSettings settings)
        {
            base.Assert(settings);

            if (settings.TotalTimeForTask < MinTotalTimeForTask)
            {
                throw new ValidationFailedException(nameof(settings.TotalTimeForTask),
                    $"Total time for task cannot be less that {MinTotalTimeForTask} seconds");
            }
        }

        protected override QuizTaskRunModel RunNewTaskImpl(SprintTaskSettings settings)
        {
            var result = base.RunNewTaskImpl(settings);

            foreach (var phrase in result.Phrases)
            {
                phrase.Translations = PhrasesService.GetTranslations(phrase.Id);
            }

            return result;
        }

        protected override IList<QuizTaskResult<Phrase>> CheckAnswers(SprintTaskSettings settings, IList<SprintTaskAnswer> result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var taskResults = new List<QuizTaskResult<Phrase>>();

            for (var i = 0; i < result.Count; i++)
            {
                var answer = result[i];
                if (answer == null)
                    throw new ArgumentNullException($"{nameof(result)}[{i}]");

                var phrase = PhrasesService.GetPhrase(answer.PhraseId);
                if (phrase == null)
                    throw new ArgumentException($"Phrase in {nameof(result)}[{i}] does not exist");

                if (phrase.LanguageId != settings.LanguageId)
                    throw new ArgumentException($"Phrase in {nameof(result)}[{i}] has different language from language in settings.");

                var isCorrect = answer.Delta > 0;

                taskResults.Add(new QuizTaskResult<Phrase>()
                {
                    Entity = new Phrase(phrase),
                    IsCorrect = isCorrect
                });
            }

            return taskResults;
        }


        //public const int MinTotalTimeForTask = 5;
        //public const int MinCountOfWordsUsed = 1;
        //public const int MaxCountOfWordsUsed = 1000;

        //private readonly IPhrasesService _phrasesService;
        //private readonly ILanguagesService _languagesService;
        //private readonly IApplicationDbContext _db;

        //public SprintTaskService(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db)
        //{
        //    _phrasesService = phrasesService;
        //    _languagesService = languagesService;
        //    _db = db;
        //}

        //public SprintTaskSettingModel GetSettings()
        //{
        //    var settingDal = _db.SprintTaskSettings.FirstOrDefault();
        //    if (settingDal == null)
        //    {
        //        // Return default one
        //        return new SprintTaskSettingModel()
        //        {
        //            LanguageId = _languagesService.GetDefaultLanguage().Id,
        //            CountOfWordsUsed = 30,
        //            TotalTimeForTask = 60
        //        };
        //    }

        //    // Return default one
        //    return new SprintTaskSettingModel()
        //    {
        //        LanguageId = settingDal.LanguageId,
        //        CountOfWordsUsed = settingDal.CountOfWordsUsed,
        //        TotalTimeForTask = settingDal.TotalTimeForTask
        //    };
        //}

        //public void SetSettings(SprintTaskSettingModel settings)
        //{
        //    /* Validate */
        //    Assert(settings);

        //    /* Save */
        //    var settingDal = _db.SprintTaskSettings.FirstOrDefault();
        //    var isNew = settingDal == null;
        //    if (isNew)
        //    {
        //        settingDal = new SprintTaskSettingDal();
        //    }

        //    settingDal.LanguageId = settings.LanguageId;
        //    settingDal.CountOfWordsUsed = settings.CountOfWordsUsed;
        //    settingDal.TotalTimeForTask = settings.TotalTimeForTask;

        //    if (isNew)
        //    {
        //        _db.SprintTaskSettings.Add(settingDal);
        //    }
        //    else
        //    {
        //        _db.MarkModified(settingDal);
        //    }
        //}

        //public SprintTaskRunModel RunNewTask(SprintTaskSettingModel settings)
        //{
        //    /* Validation */
        //    Assert(settings);

        //    /* Logic */
        //    var phrases = _db.Phrases
        //        .Where(p => p.LanguageId == settings.LanguageId)
        //        .OrderBy(
        //            p => p.SprintTaskJournalRecords.Sum(r => (int?)(r.CorrectWrongAnswersDelta)) ?? 0
        //        )
        //        .Take(settings.CountOfWordsUsed)
        //        .ToList();

        //    var result = new SprintTaskRunModel()
        //    {
        //        Phrases = phrases.Select(p => new PhraseWithTranslations(p)
        //        {
        //            Translations = _phrasesService.GetTranslations(p.Id)
        //        }).ToList()
        //    };

        //    return result;
        //}

        //public void FinishTask(SprintTaskFinishedSummaryModel summary)
        //{
        //    /* Validation */
        //    Assert(summary);

        //    /* Logic */
        //    foreach (var record in summary.Results)
        //    {
        //        var delta = record.CorrectAnswersCount - record.WrongAnswersCount;

        //        var recordDal = _db.SprintTaskJournal.FirstOrDefault(e => e.PhraseId == record.PhraseId);
        //        var isNew = recordDal == null;
        //        if (isNew)
        //        {
        //            recordDal = new SprintTaskJournalRecordDal();
        //        }

        //        recordDal.PhraseId = record.PhraseId;
        //        recordDal.LastRepetitonTime = DateTime.UtcNow;
        //        recordDal.CorrectWrongAnswersDelta += delta;

        //        if (isNew)
        //        {
        //            _db.SprintTaskJournal.Add(recordDal);
        //        }
        //        else
        //        {
        //            _db.MarkModified(recordDal);
        //        }
        //    }
        //}

        //private void Assert(SprintTaskSettingModel settings)
        //{
        //    if (settings == null)
        //        throw new ArgumentNullException(nameof(settings));

        //    if (!_languagesService.CheckIfLanguageExists(settings.LanguageId))
        //        throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

        //    if (settings.TotalTimeForTask < MinTotalTimeForTask)
        //    {
        //        throw new ValidationFailedException(nameof(settings.TotalTimeForTask),
        //            $"Total time for task cannot be less that {MinTotalTimeForTask} seconds");
        //    }

        //    if (settings.CountOfWordsUsed < MinCountOfWordsUsed || settings.CountOfWordsUsed > MaxCountOfWordsUsed)
        //    {
        //        throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
        //            $"Count of words used must be between {MinCountOfWordsUsed} and {MaxCountOfWordsUsed} words");
        //    }
        //}

        //private void Assert(SprintTaskFinishedSummaryModel summary)
        //{
        //    if (summary == null)
        //        throw new ArgumentNullException(nameof(summary));

        //    if (summary.Results == null)
        //        throw new ArgumentNullException(nameof(summary.Results));

        //    for (var i = 0; i < summary.Results.Count; i++)
        //    {
        //        var record = summary.Results[i];

        //        if (record == null)
        //            throw new ArgumentNullException($"Results[{i}]");

        //        var phraseId = record.PhraseId;

        //        if (!_phrasesService.CheckIfPhraseExists(phraseId))
        //            throw new ValidationFailedException($"Results[{i}]", $"Phrase with id '{phraseId}' does not exists");

        //        string message;
        //        if (!ValidateAnswersCount(record.CorrectAnswersCount, out message))
        //            throw new ValidationFailedException($"Results[{i}].{nameof(record.CorrectAnswersCount)}", message);
        //        if (!ValidateAnswersCount(record.WrongAnswersCount, out message))
        //            throw new ValidationFailedException($"Results[{i}].{nameof(record.WrongAnswersCount)}", message);
        //    }
        //}

        //private bool ValidateAnswersCount(int answersCount, out string message)
        //{
        //    message = string.Empty;

        //    if (answersCount < 0)
        //    {
        //        message = "Answers count cannot be less than 0";
        //        return false;
        //    }

        //    return true;
        //}
    }
}