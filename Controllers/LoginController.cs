using Microsoft.AspNetCore.Mvc;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Welp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ApplicationDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var username = model.Username;
                    var password = model.Password;

                    var user = await _context.Users
                        .FromSqlRaw("SELECT * FROM Users WHERE Username = {0} AND Password = {1}", username, password)
                        .FirstOrDefaultAsync();

                    if (user != null)
                    {
                        return RedirectToAction("Index", "Homepage");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return RedirectToAction("Index", "RegisterForm");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during user authentication.");
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                }
            }

            // If we got this far, something failed; redisplay the form
            return RedirectToAction("Index", "Index");
        }
    }

}