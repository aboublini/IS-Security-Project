using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class ProfessorModel : UserModel
    {
        [Required]
        [RegularExpression(@"(\w|\s)*")]
        public string? OfficeAddress { get; set; }
        [Required]
        [RegularExpression(@"(\w|\s)*")]
        public string? Technology { get; set; }
        [Required]
        [RegularExpression(@"\w*")]
        public string? Language { get; set; }

    }
}
