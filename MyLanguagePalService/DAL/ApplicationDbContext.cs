using System.Data.Entity;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }
    }
}