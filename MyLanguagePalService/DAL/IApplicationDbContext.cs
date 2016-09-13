using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<LanguageDal> Languages { get; set; }

        DbEntityEntry Entry(object entity);

        int SaveChanges();
    }
}