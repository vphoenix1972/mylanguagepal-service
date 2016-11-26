namespace MyLanguagePalService.DAL.Models
{
    public class TranslationDal
    {
        public int Id { get; set; }

        public int ForPhraseId { get; set; }

        public int TranslationPhraseId { get; set; }

        public int Prevalence { get; set; }

        public PhraseDal ForPhrase { get; set; }

        public PhraseDal TranslationPhrase { get; set; }
    }
}