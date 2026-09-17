using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace PairOfEmployees.Web.Models
{
    public class UploadModel : IValidatableObject
    {
        [Required(ErrorMessage = "Please select a file.")]
        public IFormFile? File { get; set; }

        public IPagedList<PairResult>? Results { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null)
            {
                yield break;
            }

            const long maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (File.Length > maxFileSize)
            {
                yield return new ValidationResult("File cannot exceed 2 MB.", new[] { nameof(File) });
            }

            var allowedExtensions = new[]
            {
                ".csv"
            };

            var extension = Path.GetExtension(File.FileName);

            if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("Only CSV files are allowed.", new[] { nameof(File) });
            }
        }
    }
}
