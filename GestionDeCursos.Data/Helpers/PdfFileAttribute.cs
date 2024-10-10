using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GestionDeCursos.Data.Helpers
{
    public class PdfFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return ValidationResult.Success;
            }

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension != ".pdf")
            {
                return new ValidationResult("Invalid file type. Please upload an pdf file.");
            }

            return ValidationResult.Success;
        }
    }
}
