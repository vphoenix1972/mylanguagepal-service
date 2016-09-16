using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public IDbSet<LanguageDal> Languages { get; set; }

        public IDbSet<PhraseDal> Phrases { get; set; }
    }
}