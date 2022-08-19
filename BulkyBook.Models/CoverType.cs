using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Cover Type")]
        [Required]
        public string? Name { get; set; }

    }


}
