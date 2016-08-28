using System.Collections.Generic;
using System.Data.Entity;

namespace MyLanguagePalService.DAL
{
    public class DebugInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var languages = new List<Language>
            {
                new Language() { Name = "English" },
                new Language() { Name = "Русский" }
            };
            languages.ForEach(language => context.Languages.Add(language));
            context.SaveChanges();
        }
    }
}