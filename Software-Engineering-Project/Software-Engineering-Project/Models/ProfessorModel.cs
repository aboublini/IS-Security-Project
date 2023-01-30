using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class ProfessorModel : UserModel
    {
        [RegularExpression(@"(\w|\s)*")]
        public string? OfficeAddress { get; set; }

        [RegularExpression(@"(\w|\s)*")]
        public string? Technology { get; set; }

        [RegularExpression(@"\w*")]
        public string? Language { get; set; }

    }
}
