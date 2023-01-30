using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class StudentModel : UserModel
    {
        [Range(1930, 2022, ErrorMessage = "You have entered an invalid year.")]
        public int? StartYear { get; set; }

        [RegularExpression(@"\w*")]
        public string? Professor { get; set; }

        
    }
}
