using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tags;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL
{
    public class ServiceManager : IServiceManager
    {
        private bool _disposed;
        private readonly IApplicationDbContext _db;

        public ServiceManager(IApplicationDbContext db, [NotNull] IFramework framework)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            if (framework == null)
                throw new ArgumentNullException(nameof(framework));

            _db = db;

            LanguagesService = new LanguagesService(_db);
            TagsService = new TagsService(_db);
            PhrasesService = new PhrasesService(_db);

            Tasks = new List<ITaskService>()
            {
                new WriteTranslationTaskService(framework, PhrasesService, LanguagesService, _db),
                new SprintTaskService(framework, PhrasesService, LanguagesService, _db)
            };
        }

        public ILanguagesService LanguagesService { get; set; }

        public ITagsService TagsService { get; private set; }

        public IPhrasesService PhrasesService { get; }

        public IList<ITaskService> Tasks { get; }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                _disposed = true;
            }
        }
    }
}