using System.Collections.Generic;

namespace MyLanguagePalService.Models.Controllers.Phrase
{
    public class DetailsVm
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public IList<string> Translations { get; set; }
    }
}