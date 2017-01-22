using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public IDbSet<LanguageDal> Languages { get; set; }

        public IDbSet<TagDal> Tags { get; set; }

        public IDbSet<PhraseDal> Phrases { get; set; }

        public IDbSet<TranslationDal> Translations { get; set; }

        public IDbSet<TaskSettingsDal> TaskSettings { get; set; }

        public IDbSet<KnowledgeLevelDal> KnowledgeLevels { get; set; }

        public void MarkModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // *** Remove cascade delete conventions ***
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            /* Configure tables */

            // Languages
            modelBuilder.Entity<LanguageDal>().ToTable("Languages");
            modelBuilder.Entity<LanguageDal>().HasKey(e => e.Id);
            modelBuilder.Entity<LanguageDal>().Property(e => e.Name).IsRequired().HasMaxLength(100);

            // Tags
            modelBuilder.Entity<TagDal>().ToTable("Tags");
            modelBuilder.Entity<TagDal>().HasKey(e => e.Id);
            modelBuilder.Entity<TagDal>().Property(e => e.Name).IsRequired().HasMaxLength(100);

            // Phrases
            modelBuilder.Entity<PhraseDal>().ToTable("Phrases");
            modelBuilder.Entity<PhraseDal>().HasKey(e => e.Id);
            modelBuilder.Entity<PhraseDal>()
                .Property(e => e.Text)
                .HasMaxLength(255)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("UX_Text") { IsUnique = true })
                );

            // Translations
            modelBuilder.Entity<TranslationDal>().ToTable("Translations");
            modelBuilder.Entity<TranslationDal>()
                .HasKey(e => e.Id);

            // Task settings
            modelBuilder.Entity<TaskSettingsDal>().ToTable("TaskSettings");
            modelBuilder.Entity<TaskSettingsDal>()
                .HasKey(e => e.Id);

            // Knowledge levels
            modelBuilder.Entity<KnowledgeLevelDal>().ToTable("KnowledgeLevels");
            modelBuilder.Entity<KnowledgeLevelDal>()
                .HasKey(e => e.Id);

            /* Configure relationships */

            // Pharses <-> Languages
            modelBuilder.Entity<LanguageDal>()
                .HasMany(l => l.Phrases)
                .WithRequired(p => p.Language)
                .HasForeignKey(p => p.LanguageId);

            // Pharses <-> Tags
            modelBuilder.Entity<TagDal>()
                .HasMany(e => e.Phrases)
                .WithMany(e => e.Tags)
                .Map(c => c.ToTable("TagsPhrases"));

            // Pharses <-> Translations
            modelBuilder.Entity<PhraseDal>()
                .HasMany(p => p.Translations)
                .WithRequired(t => t.ForPhrase)
                .HasForeignKey(t => t.ForPhraseId);

            modelBuilder.Entity<PhraseDal>()
                .HasMany(p => p.PhrasesTranslatedBy)
                .WithRequired(t => t.TranslationPhrase)
                .HasForeignKey(t => t.TranslationPhraseId);

            // Knowledge levels <-> Phrases
            modelBuilder.Entity<PhraseDal>()
                .HasMany(e => e.KnowledgeLevels)
                .WithRequired(e => e.Phrase)
                .HasForeignKey(e => e.PhraseId);
        }
    }
}