using System;
using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public interface IApplicationDbContext : IDisposable
    {
        IDbSet<LanguageDal> Languages { get; set; }

        IDbSet<PhraseDal> Phrases { get; set; }

        IDbSet<TranslationDal> Translations { get; set; }

        IDbSet<TaskSettingsDal> TaskSettings { get; set; }

        IDbSet<KnowledgeLevelDal> KnowledgeLevels { get; set; }

        int SaveChanges();

        void MarkModified(object entity);
    }
}