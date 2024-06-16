using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Welp.Pages
{
    public class madeit : PageModel
    {
        private readonly ILogger<madeit> _logger;

        public madeit(ILogger<madeit> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
