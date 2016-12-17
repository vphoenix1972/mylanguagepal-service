using System;
using System.Linq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public class SprintTaskService : ServiceBase,
                                     ISprintTaskService
    {
        private const int MinTotalTimeForTask = 5;
        private const int MinCountOfWordsUsed = 1;
        private const int MaxCountOfWordsUsed = 1000;

        private readonly IPhrasesService _phrasesService;
        private readonly ILanguagesService _languagesService;
        private readonly IApplicationDbContext _db;

        public SprintTaskService(IPhrasesService phrasesService, ILanguagesService languagesService, IApplicationDbContext db)
        {
            _phrasesService = phrasesService;
            _languagesService = languagesService;
            _db = db;
        }

        public SprintTaskSettingModel GetSettings()
        {
            var settingDal = _db.SprintTaskSettings.FirstOrDefault();
            if (settingDal == null)
            {
                // Return default one
                return new SprintTaskSettingModel()
                {
                    LanguageId = _languagesService.GetDefaultLanguage().Id,
                    CountOfWordsUsed = 30,
                    TotalTimeForTask = 60
                };
            }

            // Return default one
            return new SprintTaskSettingModel()
            {
                LanguageId = settingDal.LanguageId,
                CountOfWordsUsed = settingDal.CountOfWordsUsed,
                TotalTimeForTask = settingDal.TotalTimeForTask
            };
        }

        public void SetSettings(SprintTaskSettingModel settings)
        {
            /* Validate */
            ValidateSettings(settings);

            /* Save */
            var settingDal = _db.SprintTaskSettings.FirstOrDefault();
            var isNew = settingDal == null;
            if (isNew)
            {
                settingDal = new SprintTaskSettingDal();
            }

            settingDal.LanguageId = settings.LanguageId;
            settingDal.CountOfWordsUsed = settings.CountOfWordsUsed;
            settingDal.TotalTimeForTask = settings.TotalTimeForTask;

            if (isNew)
            {
                _db.SprintTaskSettings.Add(settingDal);
            }
            else
            {
                _db.MarkModified(settingDal);
            }
        }

        public SprintTaskRunModel RunNewTask(SprintTaskSettingModel settings)
        {
            /* Validation */
            ValidateSettings(settings);

            /* Logic */
            var phrases = _db.Phrases
                .Where(p => p.LanguageId == settings.LanguageId)
                .OrderByDescending(
                    p => p.SprintTaskJournalRecords.Sum(r => (int?)(r.CorrectAnswersCount - r.WrongAnswersCount)) ?? 0
                )
                .Take(settings.CountOfWordsUsed)
                .ToList();

            var result = new SprintTaskRunModel()
            {
                Phrases = phrases.Select(p => new PhraseWithTranslations(p)
                {
                    Translations = _phrasesService.GetTranslations(p.Id)
                }).ToList()
            };

            return result;
        }

        private void ValidateSettings(SprintTaskSettingModel settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!_languagesService.CheckIfLanguageExists(settings.LanguageId))
                throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

            if (settings.TotalTimeForTask < MinTotalTimeForTask)
            {
                throw new ValidationFailedException(nameof(settings.TotalTimeForTask),
                    $"Total time for task cannot be less that {MinTotalTimeForTask} seconds");
            }

            if (settings.CountOfWordsUsed < MinCountOfWordsUsed || settings.CountOfWordsUsed > MaxCountOfWordsUsed)
            {
                throw new ValidationFailedException(nameof(settings.CountOfWordsUsed),
                    $"Count of words used must be between {MinCountOfWordsUsed} and {MaxCountOfWordsUsed} words");
            }
        }
    }
}