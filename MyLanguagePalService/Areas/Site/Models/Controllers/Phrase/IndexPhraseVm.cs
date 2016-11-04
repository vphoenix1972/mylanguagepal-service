using System.ComponentModel.DataAnnotations;

namespace MyLanguagePalService.Areas.Site.Models.Controllers.Phrase
{
    public class IndexPhraseVm
    {
        public int Id { get; set; }

        [Display(Name = "Phrase")]
        // Use custom validation
        public string Text { get; set; }
    }
}