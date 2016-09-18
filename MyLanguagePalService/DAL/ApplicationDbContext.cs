using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public IDbSet<LanguageDal> Languages { get; set; }

        public IDbSet<PhraseDal> Phrases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Phrase has Language FK Required
            // Disable cascade deletion, but leave integrity checking
            modelBuilder.Entity<PhraseDal>()
                .HasRequired(p => p.Language)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}