using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Welp.Pages
{
    public class Forgot_Password : PageModel
    {
        private readonly ILogger<Forgot_Password> _logger;

        public Forgot_Password(ILogger<Forgot_Password> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
