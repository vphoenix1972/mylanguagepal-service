namespace MyLanguagePalService.Models.Controllers.Phrase
{
    public class CreateIm
    {
        public int? LanguageId { get; set; }

        public string Text { get; set; }

        public string Translations { get; set; }

        public string Synonims { get; set; }
    }
}