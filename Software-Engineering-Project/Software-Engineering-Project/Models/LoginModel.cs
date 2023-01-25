using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"\w*")]
        public string? Username { get; set; }

        [Required]
        //Password must contain at least one uppercase letter, one lowercase letter,
        //one number and one special character and contain at least 6 charachters
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")]
        public string? Password { get; set; }

        public bool IsLoginConfirmed { get; set; } = true;
    }
}
