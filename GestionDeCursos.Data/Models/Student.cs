using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeCursos.Data.Models
{
    [Table("Students", Schema = "Management")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Name")]
        public string? StudentName { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        [Required]
        [Display(Name = "Fee")]
        [DataType(DataType.Currency)]
        public int CourseFee { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int CourseDuration { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime CourseStartDate { get; set; }

        [Required]
        [Display(Name = "Batch Timing")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime BatchTime { get; set; }
    }
}
