using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLanguagePalService.DAL.Models
{
    [Table("Phrases")]
    public class PhraseDal
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Text { get; set; }

        public virtual LanguageDal Language { get; set; }
    }
}