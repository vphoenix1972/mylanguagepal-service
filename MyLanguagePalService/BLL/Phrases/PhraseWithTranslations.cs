using System.Collections.Generic;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public class PhraseWithTranslations : Phrase
    {
        public PhraseWithTranslations()
        {

        }

        public PhraseWithTranslations([NotNull] Phrase other) :
            base(other)
        {

        }

        public PhraseWithTranslations([NotNull] PhraseDal dal) :
            base(dal)
        {

        }

        public IList<Translation> Translations { get; set; }
    }
}