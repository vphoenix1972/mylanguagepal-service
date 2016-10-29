using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLanguagePalService.Models.Controllers.Phrase
{
    public class DetailsVm
    {
        public int Id { get; set; }

        [Display(Name = "Phrase")]
        public string Text { get; set; }

        [Display(Name = "Translations")]
        public IEnumerable<string> Translations { get; set; }

        [Display(Name = "Synonims")]
        public IList<string> Synonims { get; set; }
    }
}