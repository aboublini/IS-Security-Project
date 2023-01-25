using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class StudentProfileModel : StudentModel
    {
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")]
        public string? NewPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")]
        public string? NewPassword1 { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})", ErrorMessage = "Invalid Phone Number.")]
        public string? NewPhone { get; set; }
    }
}
