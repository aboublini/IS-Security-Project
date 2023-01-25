using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Software_Engineering_Project.Models
{
    public class MeetingModel
    {
        [RegularExpression(@"\w*")]
        public string? Professor { get; set; }
        [Required]
        [RegularExpression(@"\w*")]
        public string? Student { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        [RegularExpression(@"01:00 hour|00:30 hour|01:30 hour|02:00 hour|Fixed")]
        public string Duration { get; set; }
        [Required]
        [RegularExpression(@"in-person|online")]
        public string Type { get; set; }
        [Required]
        [RegularExpression(@"\w*")]
        public string Title { get; set; }

    }
}
