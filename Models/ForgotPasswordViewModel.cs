using System.ComponentModel.DataAnnotations;

namespace Welp.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Username { get; set; }
    }
}