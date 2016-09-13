using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<LanguageDal> Languages { get; set; }
    }
}