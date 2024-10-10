
using GestionDeCursos.Data.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeCursos.Data.Models
{
    [Table("Courses", Schema = "Management")]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Course")]
        public string? CourseName { get; set; }

        [StringLength(250)]
        [Display(Name = "Description")]
        public string? CourseDescription { get; set; }

        [StringLength(100)]
        public string? MongoFileId { get; set; }

        [NotMapped]
        public bool OverwriteFile { get; set; }

        [NotMapped]
        [Display(Name = "Upload Excel File")]
        [ExcelFile]
        public IFormFile? ExcelFileData { get; set; }
    }
}
