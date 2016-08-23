using System.ComponentModel.DataAnnotations;

namespace MyLanguagePalService.DAL
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}