using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLanguagePalService.DAL.Models
{
    [Table("Languages")]
    public class LanguageDal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<PhraseDal> Phrases { get; set; }
    }
}