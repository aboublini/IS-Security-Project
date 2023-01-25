using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class UserModel
    {
        [Required]
        [RegularExpression(@"\w*")]
        public string? Username { get; set; }

        [Required]
        //Password must contain at least one uppercase letter, one lowercase letter, one number and one special character
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string? Password { get; set; }

        [Required]
        [RegularExpression(@"([a-z]|[A-Z]|-)*")]
        public string? FirstName { get; set; }

        [Required]
        [RegularExpression(@"([a-z]|[A-Z]|-)*")]
        public string? LastName { get; set; }

        [Required]
        [RegularExpression(@"male|female|other", ErrorMessage = "Please enter 'male' , 'female' or 'other'.")]
        public string? Gender { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})", ErrorMessage = "Invalid Phone Number.")]
        public string? Phone { get; set; }

        [RegularExpression(@"student|professor")]
        public string? Role { get; set; } = "student";
    }
}
