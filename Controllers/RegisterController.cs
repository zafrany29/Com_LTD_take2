using Microsoft.AspNetCore.Mvc;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;

namespace Welp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }


       [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

   
        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Country = model.Country
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                // Redirect to "Forgot_Password" page upon successful registration
                return RedirectToAction("Index", "madeit");
            }
            return View(model);
        }
    }
}