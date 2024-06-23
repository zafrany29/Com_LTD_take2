using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Welp.Pages
{
    public class Homepage : PageModel
    {
        private readonly ILogger<Homepage> _logger;

        public Homepage(ILogger<Homepage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
