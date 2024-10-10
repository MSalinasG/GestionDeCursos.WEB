using GestionDeCursos.Data.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeCursos.Data.Models
{
    [Table("Applicants", Schema = "Enrollment")]
    public class Applicants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string? Apellido { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "El DNI no puede exceder los 8 caracteres.")]
        public string? Dni { get; set; }         

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Nacimiento { get; set; }

        [StringLength(100)]
        public string? FichaPdfMongoFileId { get; set; }

        [NotMapped]
        public bool OverwriteFile { get; set; }

        [NotMapped]
        [Display(Name = "Upload PDF File")]
        [PdfFile]
        public IFormFile? PdfFileData { get; set; }
    }
}
