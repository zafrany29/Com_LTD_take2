//using Microsoft.AspNetCore.Mvc;
//using Welp.Models;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using Welp.Data;

//namespace Welp.Controllers
//{
//    public class LoginController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public LoginController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpPost]
//        public IActionResult Index(LoginViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View("/Pages/Index.cshtml", model);
//            }

//            // Hash the input password using SHA-1
//            using (SHA1 sha1 = SHA1.Create())
//            {
//                var passwordBytes = Encoding.UTF8.GetBytes(model.Password);
//                var hashedBytes = sha1.ComputeHash(passwordBytes);
//                var hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

//                // Check if user exists
//                var user = _context.Users
//                    .FirstOrDefault(u => u.Username == model.Username && u.Password == hashedPassword);

//                if (user != null)
//                {
//                    // Redirect to home page
//                    return RedirectToPage("/HomePage");
//                }
//                else
//                {
//                    // Add model error and return view with error message
//                    ModelState.AddModelError(string.Empty, "Username or Password are incorrect");
//                    return View("/Pages/Index.cshtml", model);
//                }
//            }
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            var model = new LoginViewModel();
//            return View("/Pages/Index.cshtml", model);
//        }
//    }
//}