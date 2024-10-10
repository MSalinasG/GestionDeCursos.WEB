using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeCursos.Data.Models
{
    [Table("Instructors", Schema = "Management")]
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Instructor Name")]
        [StringLength(150)]
        public string? InstructorName { get; set; }

        [Required]
        [StringLength(100)]
        public string? Qualification { get; set; }

        [Required]
        public int Experience { get; set; }
    }
}
