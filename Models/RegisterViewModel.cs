using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Welp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "The password must be at least 10 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{10,}$",
            ErrorMessage = "The password must include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [CustomDictionaryValidator(ErrorMessage = "Password must not contain common dictionary words.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }
    }

    // Custom validator for checking dictionary words
    public class CustomDictionaryValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string password = value.ToString().ToLower(); // convert to lowercase for comparison
                string[] commonWords = { "password", "123456", "qwerty" }; // Add more common words as needed

                foreach (var word in commonWords)
                {
                    if (password.Contains(word))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}