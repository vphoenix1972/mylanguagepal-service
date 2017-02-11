using System.Collections.Generic;

namespace MyLanguagePalService.Areas.App.Models.Controller.PhrasesApi
{
    public class PhrasesApiCreateIm
    {
        public int? LanguageId { get; set; }

        public string Text { get; set; }

        public string Definition { get; set; }

        public IList<TranslationIm> Translations { get; set; }
    }
}