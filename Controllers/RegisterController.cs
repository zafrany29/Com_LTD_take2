//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Welp.Data;
//using Welp.Models;
//using System.Threading.Tasks;

//namespace Welp.Controllers
//{
//    public class RegisterController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public RegisterController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            // Initialize with default values
//            var model = new RegisterViewModel
//            {
//                Username = string.Empty,
//                Email = string.Empty,
//                Password = string.Empty,
//                PhoneNumber = string.Empty,
//                City = string.Empty,
//                Country = string.Empty
//            };

//            // Set ViewData
//            ViewData["Title"] = "Register";

//            return View("~/Pages/RegisterForm.cshtml", model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                // Check if the user already exists
//                var existingUser = await _context.Users
//                    .FirstOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);

//                if (existingUser != null)
//                {
//                    ModelState.AddModelError("", "User already exists, please try again.");
//                    ViewData["Title"] = "Register"; // Set ViewData
//                    return View("~/Pages/RegisterForm.cshtml", model);
//                }

//                var user = new User
//                {
//                    Username = model.Username,
//                    Email = model.Email,
//                    Password = model.Password,
//                    PhoneNumber = model.PhoneNumber,
//                    City = model.City,
//                    Country = model.Country
//                };

//                _context.Add(user);
//                await _context.SaveChangesAsync();

//                // Redirect to "Homepage" upon successful registration
//                return RedirectToAction("Index", "Homepage");
//            }

//            ViewData["Title"] = "Register"; // Set ViewData
//            return View("~/Pages/RegisterForm.cshtml", model);
//        }
//    }
//}