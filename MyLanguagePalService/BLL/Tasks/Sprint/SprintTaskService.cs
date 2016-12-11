using System;
using System.Data.Entity;
using System.Linq;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public class SprintTaskService : ServiceBase,
                                     ISprintTaskService
    {
        private readonly ILanguagesService _languagesService;
        private readonly IApplicationDbContext _db;

        public SprintTaskService(ILanguagesService languagesService, IApplicationDbContext db)
        {
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
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!_languagesService.CheckIfLanguageExists(settings.LanguageId))
                throw new ValidationFailedException(nameof(settings.LanguageId), GetLanguageNotExistString(settings.LanguageId));

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
                _db.Entry(settingDal).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public SprintTaskData GetDataForNewTask()
        {
            throw new NotImplementedException();
        }
    }
}