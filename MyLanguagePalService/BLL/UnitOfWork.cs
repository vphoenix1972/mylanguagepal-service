﻿using System;
using MyLanguagePalService.BLL.Languages;
using MyLanguagePalService.BLL.Tasks.Sprint;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly IApplicationDbContext _db;

        public UnitOfWork(IApplicationDbContext db)
        {
            _db = db;

            LanguagesService = new LanguagesService(_db);
            PhrasesService = new PhrasesService(_db);
            SprintTaskService = new SprintTaskService(LanguagesService, _db);
        }

        public ILanguagesService LanguagesService { get; set; }

        public IPhrasesService PhrasesService { get; }

        public ISprintTaskService SprintTaskService { get; set; }

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