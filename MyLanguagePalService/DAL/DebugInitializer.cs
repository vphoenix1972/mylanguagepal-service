// DropCreateDatabaseIfModelChanges doesn't work if a migration exists
// So enable migrations compilation only in release mode
// See http://stackoverflow.com/questions/19430502/dropcreatedatabaseifmodelchanges-ef6-results-in-system-invalidoperationexception

#if DEBUG
using System.Collections.Generic;
using System.Data.Entity;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL
{
    public class DebugInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var languages = new List<LanguageDal>
            {
                new LanguageDal() { Name = "English" },
                new LanguageDal() { Name = "Русский" }
            };
            languages.ForEach(language => context.Languages.Add(language));
            context.SaveChanges();
        }
    }
}
#endif