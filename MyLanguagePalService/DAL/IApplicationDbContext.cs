using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public interface IApplicationDbContext : IDisposable
    {
        IDbSet<LanguageDal> Languages { get; set; }

        IDbSet<PhraseDal> Phrases { get; set; }

        IDbSet<TranslationDal> Translations { get; set; }

        IDbSet<SprintTaskSettingDal> SprintTaskSettings { get; set; }

        DbEntityEntry Entry(object entity);

        int SaveChanges();

        void MarkModified(object entity);
    }
}