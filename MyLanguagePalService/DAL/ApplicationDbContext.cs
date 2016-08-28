using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LanguageDal> Languages { get; set; }
    }
}