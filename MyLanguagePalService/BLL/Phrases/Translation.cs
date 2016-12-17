using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Phrases
{
    public class Translation
    {
        [NotNull]
        public Phrase Phrase { get; set; } = new Phrase();

        public int Prevalence { get; set; }
    }
}