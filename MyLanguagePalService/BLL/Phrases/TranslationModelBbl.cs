using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Phrases
{
    public class TranslationModelBbl
    {
        public PhraseDal Phrase { get; set; }

        public int Prevalence { get; set; }
    }
}