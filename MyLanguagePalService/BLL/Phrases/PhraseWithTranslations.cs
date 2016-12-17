using System.Collections.Generic;
using JetBrains.Annotations;

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

        public IList<Translation> Translations { get; set; }

        [NotNull]
        public static PhraseWithTranslations MapFrom([NotNull] Phrase phrase)
        {
            return new PhraseWithTranslations(phrase);
        }
    }
}