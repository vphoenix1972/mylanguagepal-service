using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Phrases
{
    public class TranslationModel
    {
        [NotNull]
        public PhraseModel Phrase { get; set; } = new PhraseModel();

        public int Prevalence { get; set; }
    }
}