using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyLanguagePal.Core.Framework;
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
        protected TaskServiceBase([NotNull] IFramework framework,
            [NotNull] IPhrasesService phrasesService,
            [NotNull] ILanguagesService languagesService,
            [NotNull] IApplicationDbContext db)
        {
            if (framework == null)
                throw new ArgumentNullException(nameof(framework));
            if (phrasesService == null)
                throw new ArgumentNullException(nameof(phrasesService));
            if (languagesService == null)
                throw new ArgumentNullException(nameof(languagesService));
            if (db == null)
                throw new ArgumentNullException(nameof(db));


            Framework = framework;
            PhrasesService = phrasesService;
            LanguagesService = languagesService;
            Db = db;
        }

        public abstract string Name { get; }

        protected abstract int TaskId { get; }

        [NotNull]
        protected IFramework Framework { get; set; }

        [NotNull]
        protected IPhrasesService PhrasesService { get; set; }

        [NotNull]
        protected ILanguagesService LanguagesService { get; set; }

        [NotNull]
        protected IApplicationDbContext Db { get; set; }

        public object GetSettings()
        {
            return GetSettingsImpl();
        }

        public object SetSettings(object settings)
        {
            var typedSettings = settings.FromJObjectTo<TSettings>();
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            Assert(typedSettings);

            Db.AddOrUpdate(
                dbSetGetter: context => context.TaskSettings,
                searcher: set => set.FirstOrDefault(dal => dal.TaskId == TaskId),
                setter: dal =>
                {
                    dal.TaskId = TaskId;
                    dal.SettingsJson = JsonConvert.SerializeObject(typedSettings);
                }
            );

            return typedSettings;
        }

        public object RunNewTask(object settings)
        {
            var typedSettings = settings.FromJObjectTo<TSettings>();
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            Assert(typedSettings);

            return RunNewTaskImpl(typedSettings);
        }

        public object FinishTask(object settings, object answers)
        {
            var typedSettings = settings.FromJObjectTo<TSettings>();
            if (typedSettings == null)
                throw new ArgumentException(nameof(settings));

            var typedAnswers = answers.FromJObjectTo<TAnswers>();
            if (typedAnswers == null)
                throw new ArgumentException(nameof(answers));

            return FinishTaskImpl(typedSettings, typedAnswers);
        }

        protected virtual TSettings GetSettingsImpl()
        {
            var settingsDal = Db.TaskSettings.FirstOrDefault(s => s.TaskId == TaskId);
            if (settingsDal == null)
                return DefaultSettings();

            return JsonConvert.DeserializeObject<TSettings>(settingsDal.SettingsJson);
        }


        protected abstract void Assert(TSettings settings);

        protected abstract TRunModel RunNewTaskImpl(TSettings settings);

        protected abstract TSummary FinishTaskImpl(TSettings settings, TAnswers answers);

        protected abstract TSettings DefaultSettings();

        protected IList<KnowledgeLevelDal> GetTaskKnowledgeLevels()
        {
            return Db.KnowledgeLevels.Where(l => l.TaskId == TaskId).ToList();
        }
    }
}