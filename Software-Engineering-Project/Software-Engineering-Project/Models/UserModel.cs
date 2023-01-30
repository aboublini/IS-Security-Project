using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class UserModel
    {
        [RegularExpression(@"\w*")]
        public string? Username { get; set; }

        //Password must contain at least one uppercase letter, one lowercase letter, one number and one special character
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")]
        public string? Password { get; set; }

        [RegularExpression(@"([a-z]|[A-Z]|-)*")]
        public string? FirstName { get; set; }

        [RegularExpression(@"([a-z]|[A-Z]|-)*")]
        public string? LastName { get; set; }

        [RegularExpression(@"male|female|other", ErrorMessage = "Please enter 'male' , 'female' or 'other'.")]
        public string? Gender { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"^([0-9]{10})", ErrorMessage = "Invalid Phone Number.")]
        public string? Phone { get; set; }

        [RegularExpression(@"student|professor")]
        public string? Role { get; set; } = "student";
    }
}
