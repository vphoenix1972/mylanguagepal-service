using System.Collections.Generic;

namespace MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi
{
    public class PhrasesApiDetailsAm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public IList<string> Translations { get; set; }
    }
}