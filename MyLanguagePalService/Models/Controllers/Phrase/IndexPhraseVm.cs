using System.ComponentModel.DataAnnotations;

namespace MyLanguagePalService.Models.Controllers.Phrase
{
    public class IndexPhraseVm
    {
        public int Id { get; set; }

        [Display(Name = "Phrase")]
        public string Text { get; set; }
    }
}