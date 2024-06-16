using System.ComponentModel.DataAnnotations;

namespace Welp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        [StringLength(20)]
        public  required string PhoneNumber { get; set; } // Added for phone number

        [Required]
        [StringLength(50)]
        public  required string City { get; set; } // Added for city

        [Required]
        [StringLength(50)]
        public required string Country { get; set; } // Added for country
    }
}
