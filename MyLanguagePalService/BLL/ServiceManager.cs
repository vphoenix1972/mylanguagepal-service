using System;
using System.Collections.Generic;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Phrases;
using MyLanguagePalService.BLL.Tasks;
using MyLanguagePalService.BLL.Tasks.Quiz;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.BLL.Tasks.WriteTranslation;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL
{
    public class ServiceManager : IServiceManager
    {
        private bool _disposed;
        private readonly IApplicationDbContext _db;

        public ServiceManager(IApplicationDbContext db)
        {
            _db = db;

            LanguagesService = new LanguagesService(_db);
            PhrasesService = new PhrasesService(_db);
            SprintTaskService = new SprintTaskService(PhrasesService, LanguagesService, _db);

            Tasks = new List<ITaskService>()
            {
                new WriteTranslationTaskService(PhrasesService, LanguagesService, _db, 1, "writeTranslation")
        };
        }

        public ILanguagesService LanguagesService { get; set; }

        public IPhrasesService PhrasesService { get; }

        public ISprintTaskService SprintTaskService { get; set; }

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