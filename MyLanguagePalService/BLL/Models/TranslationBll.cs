using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Models
{
    public class TranslationBll
    {
        public PhraseDal Phrase { get; set; }

        public int Prevalence { get; set; }
    }
}