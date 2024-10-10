using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDeCursos.Data.Models
{
    [Table("Roles", Schema = "Auth")]
    public class AppRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(60)]
        public string? Name { get; set; }
    }
}
