using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GestionDeCursos.Data.Helpers
{
    public class ExcelFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return ValidationResult.Success;
            }

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension != ".xls" && extension != ".xlsx")
            {
                return new ValidationResult("Invalid file type. Please upload an Excel file.");
            }

            return ValidationResult.Success;
        }
    }
}
