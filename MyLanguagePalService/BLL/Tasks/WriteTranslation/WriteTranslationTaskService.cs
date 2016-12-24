using System;
using System.Linq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public class WriteTranslationTaskService : ServiceBase,
                                               IWriteTranslationTaskService
    {
        public const int MinCountOfWordsUsed = 1;
        public const int MaxCountOfWordsUsed = 1000;
        public const int DefaultCountOfWordsUsed = 30;

        private readonly IPhrasesService _phrasesService;
        private readonly ILanguagesService _languagesService;
        private readonly IApplicationDbContext _db;

        public WriteTranslationTaskService(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db)
        {
            _phrasesService = phrasesService;
            _languagesService = languagesService;
            _db = db;
        }

        public WriteTranslationTaskSettingModel GetSettings()
        {
            var settingDal = _db.WriteTranslationTaskSettings.FirstOrDefault();
            if (settingDal == null)
            {
                // Return default one
                return new WriteTranslationTaskSettingModel()
                {
                    LanguageId = _languagesService.GetDefaultLanguage().Id,
                    CountOfWordsUsed = DefaultCountOfWordsUsed
                };
            }

            // Return default one
            return new WriteTranslationTaskSettingModel()
            {
                LanguageId = settingDal.LanguageId,
                CountOfWordsUsed = settingDal.CountOfWordsUsed
            };
        }

        public void SetSettings(WriteTranslationTaskSettingModel settings)
        {
            /* Validate */
            Assert(settings);

            /* Save */
            var settingDal = _db.WriteTranslationTaskSettings.FirstOrDefault();
            var isNew = settingDal == null;
            if (isNew)
            {
                settingDal = new WriteTranslationTaskSettingDal();
            }

            settingDal.LanguageId = settings.LanguageId;
            settingDal.CountOfWordsUsed = settings.CountOfWordsUsed;

            if (isNew)
            {
                _db.WriteTranslationTaskSettings.Add(settingDal);
            }
            else
            {
                _db.MarkModified(settingDal);
            }
        }

        public WriteTranslationTaskRunModel RunNewTask(WriteTranslationTaskSettingModel settings)
        {
            /* Validation */
            Assert(settings);

            /* Logic */
            var phrases = _db.Phrases
                .Where(p => p.LanguageId == settings.LanguageId)
                .OrderBy(
                    p => p.WriteTranslationTaskJournalRecords.Sum(r => (int?)(r.CorrectWrongAnswersDelta)) ?? 0
                )
                .Take(settings.CountOfWordsUsed)
                .ToList();

            var result = new WriteTranslationTaskRunModel()
            {
                Phrases = phrases.Select(p => new PhraseWithTranslations(p)
                {
                    Translations = _phrasesService.GetTranslations(p.Id)
                }).ToList()
            };

            return result;
        }

        public void FinishTask(WriteTranslationTaskFinishedSummaryModel summary)
        {
            /* Validation */
            Assert(summary);

            /* Logic */
            foreach (var record in summary.Results)
            {
                var delta = record.CorrectAnswersCount - record.WrongAnswersCount;

                var recordDal = _db.WriteTranslationTaskJournal.FirstOrDefault(e => e.PhraseId == record.PhraseId);
                var isNew = recordDal == null;
                if (isNew)
                {
                    recordDal = new WriteTranslationTaskJournalRecordDal();
                }

                recordDal.PhraseId = record.PhraseId;
                recordDal.LastRepetitonTime = DateTime.UtcNow;
                recordDal.CorrectWrongAnswersDelta += delta;

                if (isNew)
                {
                    _db.WriteTranslationTaskJournal.Add(recordDal);
                }
                else
                {
                    _db.MarkModified(recordDal);
                }
            }
        }

        private void Assert(WriteTranslationTaskSettingModel settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!_languagesService.CheckIfLanguageExists(settings.LanguageId))
                throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

            if (settings.CountOfWordsUsed < MinCountOfWordsUsed || settings.CountOfWordsUsed > MaxCountOfWordsUsed)
            {
                throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
                    $"Count of words used must be between {MinCountOfWordsUsed} and {MaxCountOfWordsUsed} words");
            }
        }

        private void Assert(WriteTranslationTaskFinishedSummaryModel summary)
        {
            if (summary == null)
                throw new ArgumentNullException(nameof(summary));

            if (summary.Results == null)
                throw new ArgumentNullException(nameof(summary.Results));

            for (var i = 0; i < summary.Results.Count; i++)
            {
                var record = summary.Results[i];

                if (record == null)
                    throw new ArgumentNullException($"Results[{i}]");

                var phraseId = record.PhraseId;

                if (!_phrasesService.CheckIfPhraseExists(phraseId))
                    throw new ValidationFailedException($"Results[{i}]", $"Phrase with id '{phraseId}' does not exists");

                string message;
                if (!ValidateAnswersCount(record.CorrectAnswersCount, out message))
                    throw new ValidationFailedException($"Results[{i}].{nameof(record.CorrectAnswersCount)}", message);
                if (!ValidateAnswersCount(record.WrongAnswersCount, out message))
                    throw new ValidationFailedException($"Results[{i}].{nameof(record.WrongAnswersCount)}", message);
            }
        }

        private bool ValidateAnswersCount(int answersCount, out string message)
        {
            message = string.Empty;

            if (answersCount < 0)
            {
                message = "Answers count cannot be less than 0";
                return false;
            }

            return true;
        }
    }
}