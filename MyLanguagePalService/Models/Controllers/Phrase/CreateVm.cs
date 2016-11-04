using System.Collections.Generic;

namespace MyLanguagePalService.Models.Controllers.Phrase
{
    public class CreateVm
    {
        public int LanguageId { get; set; }

        public string Text { get; set; }

        public string Translations { get; set; }

        public string Synonims { get; set; }

        public IList<LanguageOptionVm> LanguageOptions { get; set; }
    }
}