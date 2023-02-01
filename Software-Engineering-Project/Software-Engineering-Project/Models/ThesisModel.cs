using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class ThesisModel : StudentModel
    {
        [Required]
        [RegularExpression(@"(\w|\s)*")]
        public string? Title { get; set; }

        [Required]
        public DateOnly ThesisStartDate { get; set; }

        
        [Range(0, 10)]
        public int? Grade { get; set; }

        [Required]
        [RegularExpression(@"([a-z]|[A-Z])*")]
        public string? Language { get; set; }

        [Required]
        [RegularExpression(@"(\w|\s)*")]
        public string? Technology { get; set; }

        public IFormFile? File { get; set; }

        [Range(1,3)]
        public int? version { get; set; }

    }
}
