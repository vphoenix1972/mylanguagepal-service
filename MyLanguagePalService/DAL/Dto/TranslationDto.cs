using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.DAL.Dto
{
    public class TranslationDto
    {
        public PhraseDal Phrase { get; set; }

        public int Prevalence { get; set; }
    }
}