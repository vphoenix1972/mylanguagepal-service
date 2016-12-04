using System.Collections.Generic;
using MyLanguagePalService.Areas.App.Models.Controller.TranslationsApi;

namespace MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi
{
    public class PhrasesApiDetailsAm
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public IList<TranslationAm> Translations { get; set; }
    }
}