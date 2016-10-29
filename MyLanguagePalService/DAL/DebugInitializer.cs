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

            var phrases = new List<PhraseDal>
            {
                new PhraseDal() { Text = "to think", Language = languages[0] },
                new PhraseDal() { Text = "to say", Language = languages[0] },
                new PhraseDal() { Text = "думать", Language = languages[1] },
                new PhraseDal() { Text = "сказать", Language = languages[1] },
                new PhraseDal() { Text = "говорить", Language = languages[1] },
            };

            phrases[0].Translations = new List<PhraseDal>()
            {
                phrases[2]
            };
            phrases[2].Translations = new List<PhraseDal>()
            {
                phrases[0]
            };
            phrases[1].Translations = new List<PhraseDal>()
            {
                phrases[3],
                phrases[4],
            };
            phrases[3].Translations = new List<PhraseDal>()
            {
                phrases[1]
            };
            phrases[4].Translations = new List<PhraseDal>()
            {
                phrases[1]
            };

            phrases.ForEach(phrase => context.Phrases.Add(phrase));

            context.SaveChanges();
        }
    }
}
#endif