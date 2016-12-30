using System;
using System.Data.Entity;
using System.Linq;
using JetBrains.Annotations;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Extensions;
using MyLanguagePalService.DAL.Models;
using Newtonsoft.Json;

namespace MyLanguagePalService.BLL.Tasks
{
    public abstract class TaskServiceBase<TSettings, TRunModel, TAnswers, TSummary> : ServiceBase,
                                                                                      ITaskService
        where TSettings : class, new() where TAnswers : class
    {
        protected TaskServiceBase([NotNull] IPhrasesService phrasesService,
            [NotNull] ILanguagesService languagesService,
            [NotNull] IApplicationDbContext db,
            int taskId,
            [NotNull] string name)
        {
            if (phrasesService == null)
                throw new ArgumentNullException(nameof(phrasesService));
            if (languagesService == null)
                throw new ArgumentNullException(nameof(languagesService));
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            PhrasesService = phrasesService;
            LanguagesService = languagesService;
            Db = db;
            TaskId = taskId;
            Name = name;
        }

        [NotNull]
        protected IPhrasesService PhrasesService { get; set; }

        [NotNull]
        protected ILanguagesService LanguagesService { get; set; }

        [NotNull]
        protected IApplicationDbContext Db { get; set; }

        protected int TaskId { get; set; }

        public string Name { get; protected set; }

        public object GetSettings()
        {
            return GetSettingsImpl();
        }

        public object SetSettings(object settings)
        {     
            var typedSettings = settings.FromJObjectTo<TSettings>();
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            return SetSettingsImpl(typedSettings);
        }

        public object RunNewTask(object settings)
        {
            var typedSettings = settings as TSettings;
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            return RunNewTaskImpl(typedSettings);
        }

        public object FinishTask(object answers)
        {
            var typedAnswers = answers as TAnswers;
            if (typedAnswers == null)
                throw new ArgumentException(nameof(answers));

            return FinishTaskImpl(typedAnswers);
        }

        protected virtual TSettings GetSettingsImpl()
        {
            var settingsDal = Db.TaskSettings.FirstOrDefault(s => s.TaskId == TaskId);
            if (settingsDal == null)
                return DefaultSettings();

            return JsonConvert.DeserializeObject<TSettings>(settingsDal.SettingsJson);
        }

        protected virtual TSettings SetSettingsImpl(TSettings settings)
        {
            Db.AddOrUpdate(
                dbSetGetter: context => context.TaskSettings,
                searcher: set => set.FirstOrDefault(dal => dal.TaskId == TaskId),
                setter: dal =>
                {
                    dal.TaskId = TaskId;
                    dal.SettingsJson = JsonConvert.SerializeObject(settings);
                }
            );

            return settings;
        }

        protected abstract TRunModel RunNewTaskImpl(TSettings settings);

        protected abstract TSummary FinishTaskImpl(TAnswers answers);

        protected abstract TSettings DefaultSettings();
    }
}