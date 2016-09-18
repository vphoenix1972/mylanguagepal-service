using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public IDbSet<LanguageDal> Languages { get; set; }

        public IDbSet<PhraseDal> Phrases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove cascade delete conventions
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Configure tables
            //modelBuilder.Entity<LanguageDal>().ToTable("Languages");
            //modelBuilder.Entity<LanguageDal>().HasKey(e => e.Id);
            //modelBuilder.Entity<LanguageDal>().Property(e => e.Name).IsRequired().HasMaxLength(100);

            //modelBuilder.Entity<PhraseDal>().ToTable("Phrases");
            //modelBuilder.Entity<PhraseDal>().HasKey(e => e.Id);
            //modelBuilder.Entity<PhraseDal>()
            //    .Property(e => e.Text)
            //    .IsMaxLength()
            //    .HasColumnType("ntext");

            //// Configure relationships
            //modelBuilder.Entity<LanguageDal>()
            //    .HasMany(l => l.Phrases)
            //    .WithRequired(p => p.Language)
            //    .HasForeignKey(p => p.LanguageId)
            //    .WillCascadeOnDelete(false);
        }
    }
}