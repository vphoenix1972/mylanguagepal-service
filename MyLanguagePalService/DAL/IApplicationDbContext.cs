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

        DbEntityEntry Entry(object entity);

        int SaveChanges();
    }
}