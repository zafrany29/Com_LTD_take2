using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Welp.Pages
{
    public class AdminHomePage : PageModel
    {
        private readonly ILogger<AdminHomePage> _logger;

        public AdminHomePage(ILogger<AdminHomePage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
