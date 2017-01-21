using System.Collections.Generic;
using System.Linq;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyLanguagePalService.DAL.ApplicationDbContext";
        }

        protected override void Seed(DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

#if DEBUG
            /* Languages */
            var languages = context.Languages.ToList();
            if (!languages.Any())
            {
                languages = new List<LanguageDal>
                {
                    new LanguageDal() { Name = "English" },
                    new LanguageDal() { Name = "Русский" }
                };
                languages.ForEach(language => context.Languages.AddOrUpdate(language));
            }

            /* Phrases */
            var phrases = context.Phrases.ToList();
            if (!phrases.Any())
            {
                phrases = new List<PhraseDal>
                {
                    new PhraseDal() { Text = "to think", Language = languages[0] }, // 0
                    new PhraseDal() { Text = "to say", Language = languages[0] }, // 1
                    new PhraseDal() { Text = "suddenly", Language = languages[0] }, // 2
                    new PhraseDal() { Text = "unexpectedly", Language = languages[0] }, // 3
                    new PhraseDal() { Text = "surprisingly", Language = languages[0] }, // 4
                    new PhraseDal() { Text = "to admit", Language = languages[0] }, // 5
                    new PhraseDal() { Text = "to confess", Language = languages[0] }, // 6
                    new PhraseDal() { Text = "to accept", Language = languages[0] }, // 7
                    new PhraseDal() { Text = "on behalf of someone", Language = languages[0] }, // 8
                    new PhraseDal() { Text = "promotion", Language = languages[0] }, // 9
                    new PhraseDal() { Text = "encouragement", Language = languages[0] }, // 10
                    new PhraseDal() { Text = "stimulation", Language = languages[0] }, // 11
                    new PhraseDal() { Text = "he was passing by", Language = languages[0] }, // 12
                    new PhraseDal() { Text = "he volunteered", Language = languages[0] }, // 13
                    new PhraseDal() { Text = "frame", Language = languages[0] }, // 14
                    new PhraseDal() { Text = "shot", Language = languages[0] }, // 15
                    new PhraseDal() { Text = "to pass along", Language = languages[0] }, // 16
                    new PhraseDal() { Text = "to pass through", Language = languages[0] }, // 17
                    new PhraseDal() { Text = "pull through", Language = languages[0] }, // 18
                    new PhraseDal() { Text = "manage", Language = languages[0] }, // 19
                    new PhraseDal() { Text = "make out", Language = languages[0] }, // 20
                    new PhraseDal() { Text = "cope", Language = languages[0] }, // 21
                    new PhraseDal() { Text = "gutter", Language = languages[0] }, // 22
                    new PhraseDal() { Text = "drain", Language = languages[0] }, // 23
                    new PhraseDal() { Text = "deflux", Language = languages[0] }, // 24
                    new PhraseDal() { Text = "refined", Language = languages[0] }, // 25
                    new PhraseDal() { Text = "purified", Language = languages[0] }, // 26
                    new PhraseDal() { Text = "cleaned", Language = languages[0] }, // 27
                    new PhraseDal() { Text = "cleaning", Language = languages[0] }, // 28
                    new PhraseDal() { Text = "purification", Language = languages[0] }, // 29
                    new PhraseDal() { Text = "to store", Language = languages[0] }, // 30
                    new PhraseDal() { Text = "to accumulate", Language = languages[0] }, // 31
                    new PhraseDal() { Text = "to gather", Language = languages[0] }, // 32
                    new PhraseDal() { Text = "gather", Language = languages[0] }, // 33
                    new PhraseDal() { Text = "to pile up", Language = languages[0] }, // 34

                    new PhraseDal() { Text = "думать", Language = languages[1] }, // 35
                    new PhraseDal() { Text = "сказать", Language = languages[1] }, // 36
                    new PhraseDal() { Text = "говорить", Language = languages[1] }, // 37
                    new PhraseDal() { Text = "вдруг", Language = languages[1] }, // 38
                    new PhraseDal() { Text = "внезапно", Language = languages[1] }, // 39
                    new PhraseDal() { Text = "неожиданно", Language = languages[1] }, // 40
                    new PhraseDal() { Text = "удивительно", Language = languages[1] }, // 41
                    new PhraseDal() { Text = "признавать", Language = languages[1] }, // 42
                    new PhraseDal() { Text = "соглашаться", Language = languages[1] }, // 43
                    new PhraseDal() { Text = "признаваться", Language = languages[1] }, // 44
                    new PhraseDal() { Text = "сознаваться", Language = languages[1] }, // 45
                    new PhraseDal() { Text = "принимать", Language = languages[1] }, // 46
                    new PhraseDal() { Text = "от имени кого-то", Language = languages[1] }, // 47
                    new PhraseDal() { Text = "поощрение", Language = languages[1] }, // 48
                    new PhraseDal() { Text = "он проходил мимо меня", Language = languages[1] }, // 49
                    new PhraseDal() { Text = "он сам вызвался", Language = languages[1] }, // 50
                    new PhraseDal() { Text = "рамка", Language = languages[1] }, // 51
                    new PhraseDal() { Text = "кадр", Language = languages[1] }, // 52
                    new PhraseDal() { Text = "рама", Language = languages[1] }, // 53
                    new PhraseDal() { Text = "каркас", Language = languages[1] }, // 54
                    new PhraseDal() { Text = "оправа", Language = languages[1] }, // 55
                    new PhraseDal() { Text = "пройти вдоль", Language = languages[1] }, // 56
                    new PhraseDal() { Text = "проходить через", Language = languages[1] }, // 57
                    new PhraseDal() { Text = "справляться", Language = languages[1] }, // 58
                    new PhraseDal() { Text = "преодолевать", Language = languages[1] }, // 59
                    new PhraseDal() { Text = "выкарабкиваться", Language = languages[1] }, // 60
                    new PhraseDal() { Text = "водосточный желоб", Language = languages[1] }, // 61
                    new PhraseDal() { Text = "желоб", Language = languages[1] }, // 62
                    new PhraseDal() { Text = "сточная канава", Language = languages[1] }, // 63
                    new PhraseDal() { Text = "водосток", Language = languages[1] }, // 64
                    new PhraseDal() { Text = "отток", Language = languages[1] }, // 65
                    new PhraseDal() { Text = "осушать", Language = languages[1] }, // 66
                    new PhraseDal() { Text = "отводный", Language = languages[1] }, // 67
                    new PhraseDal() { Text = "очищенный", Language = languages[1] }, // 68
                    new PhraseDal() { Text = "рафинированный", Language = languages[1] }, // 69
                    new PhraseDal() { Text = "убранный", Language = languages[1] }, // 70
                    new PhraseDal() { Text = "уборка", Language = languages[1] }, // 71
                    new PhraseDal() { Text = "чистка", Language = languages[1] }, // 72
                    new PhraseDal() { Text = "чистящий", Language = languages[1] }, // 73
                    new PhraseDal() { Text = "очищающий", Language = languages[1] }, // 74
                    new PhraseDal() { Text = "очистка", Language = languages[1] }, // 75
                    new PhraseDal() { Text = "очищение", Language = languages[1] }, // 76
                    new PhraseDal() { Text = "хранить", Language = languages[1] }, // 77
                    new PhraseDal() { Text = "складировать", Language = languages[1] }, // 78
                    new PhraseDal() { Text = "накапливать", Language = languages[1] }, // 79
                    new PhraseDal() { Text = "аккумулировать", Language = languages[1] }, // 80
                    new PhraseDal() { Text = "скапливаться", Language = languages[1] }, // 81
                    new PhraseDal() { Text = "собирать", Language = languages[1] }, // 82
                    new PhraseDal() { Text = "собираться", Language = languages[1] }, // 83
                    new PhraseDal() { Text = "сбор", Language = languages[1] }, // 84
                    new PhraseDal() { Text = "урожай", Language = languages[1] }, // 85
                    new PhraseDal() { Text = "взгромоздить", Language = languages[1] } // 86
                };

                phrases.ForEach(phrase => context.Phrases.AddOrUpdate(phrase));
            }


            /* Translations */
            var translations = context.Translations.ToList();
            if (!translations.Any())
            {
                translations = new List<TranslationDal>
                {
                    new TranslationDal() { ForPhrase = phrases[0], TranslationPhrase = phrases[35], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[1], TranslationPhrase = phrases[36], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[1], TranslationPhrase = phrases[37], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[2], TranslationPhrase = phrases[38], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[2], TranslationPhrase = phrases[39], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[2], TranslationPhrase = phrases[40], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[3], TranslationPhrase = phrases[38], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[3], TranslationPhrase = phrases[39], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[3], TranslationPhrase = phrases[40], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[4], TranslationPhrase = phrases[41], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[4], TranslationPhrase = phrases[40], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[5], TranslationPhrase = phrases[42], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[5], TranslationPhrase = phrases[43], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[6], TranslationPhrase = phrases[44], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[6], TranslationPhrase = phrases[45], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[7], TranslationPhrase = phrases[46], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[7], TranslationPhrase = phrases[43], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[8], TranslationPhrase = phrases[47], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[9], TranslationPhrase = phrases[48], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[10], TranslationPhrase = phrases[48], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[11], TranslationPhrase = phrases[48], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[12], TranslationPhrase = phrases[49], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[13], TranslationPhrase = phrases[50], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[14], TranslationPhrase = phrases[51], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[14], TranslationPhrase = phrases[52], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[14], TranslationPhrase = phrases[53], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[14], TranslationPhrase = phrases[54], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[14], TranslationPhrase = phrases[55], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[15], TranslationPhrase = phrases[52], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[16], TranslationPhrase = phrases[56], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[17], TranslationPhrase = phrases[56], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[17], TranslationPhrase = phrases[57], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[17], TranslationPhrase = phrases[59], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[18], TranslationPhrase = phrases[59], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[19], TranslationPhrase = phrases[58], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[19], TranslationPhrase = phrases[59], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[20], TranslationPhrase = phrases[58], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[20], TranslationPhrase = phrases[59], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[21], TranslationPhrase = phrases[58], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[21], TranslationPhrase = phrases[59], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[21], TranslationPhrase = phrases[60], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[22], TranslationPhrase = phrases[61], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[22], TranslationPhrase = phrases[62], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[23], TranslationPhrase = phrases[60], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[23], TranslationPhrase = phrases[64], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[23], TranslationPhrase = phrases[65], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[23], TranslationPhrase = phrases[66], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[23], TranslationPhrase = phrases[67], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[24], TranslationPhrase = phrases[65], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[25], TranslationPhrase = phrases[64], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[25], TranslationPhrase = phrases[68], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[25], TranslationPhrase = phrases[69], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[26], TranslationPhrase = phrases[68], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[27], TranslationPhrase = phrases[68], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[27], TranslationPhrase = phrases[70], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[71], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[72], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[73], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[74], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[75], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[28], TranslationPhrase = phrases[76], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[29], TranslationPhrase = phrases[76], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[29], TranslationPhrase = phrases[75], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[29], TranslationPhrase = phrases[72], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[30], TranslationPhrase = phrases[77], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[30], TranslationPhrase = phrases[78], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[30], TranslationPhrase = phrases[79], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[31], TranslationPhrase = phrases[80], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[31], TranslationPhrase = phrases[81], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[32], TranslationPhrase = phrases[82], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[32], TranslationPhrase = phrases[83], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[32], TranslationPhrase = phrases[81], Prevalence = 30 },
                    new TranslationDal() { ForPhrase = phrases[33], TranslationPhrase = phrases[85], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[33], TranslationPhrase = phrases[84], Prevalence = 20 },
                    new TranslationDal() { ForPhrase = phrases[34], TranslationPhrase = phrases[86], Prevalence = 40 },
                    new TranslationDal() { ForPhrase = phrases[34], TranslationPhrase = phrases[81], Prevalence = 20 }
                };

                translations.ForEach(translation => context.Translations.AddOrUpdate(translation));
            }

            context.SaveChanges();
#endif
        }
    }
}