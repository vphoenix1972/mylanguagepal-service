using System.ComponentModel.DataAnnotations;

namespace MyLanguagePalService.Models.Controllers.Phrase
{
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

    public class PhraseVm
    {
        public int Id { get; set; }

        [Display(Name = "Language")]
        [Required]
        public int? LanguageId { get; set; }

        [Display(Name = "Phrase")]
        [Required]
        [StringLength(100, ErrorMessage = "Phrase is too long")]
        public string Text { get; set; }

        [Display(Name = "Translations")]
        [Required]
        [StringLength(1000, ErrorMessage = "Translations is too long")]
        public string Translations { get; set; }

        [Display(Name = "Synonims")]
        [StringLength(1000, ErrorMessage = "Synonims is too long")]
        public string Synonims { get; set; }
    }
}