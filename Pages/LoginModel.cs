using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace Welp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();

        public void OnGet()
        {
            // Initialize with default values if needed
            LoginViewModel = new LoginViewModel(); // Ensure the view model is not null
        }

        private string ComputeHmacHash(string input, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = hmac.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user by username
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == LoginViewModel.Username);

                if (existingUser != null)
                {
                    // Hash the input password with the user's salt
                    string hashedPassword = ComputeHmacHash(LoginViewModel.Password, existingUser.Salt);

                    // Check if the hashed password matches the stored password
                    if (existingUser.Password == hashedPassword)
                    {
                        string redirectionPath = "/Homepage";

                        switch (existingUser.UserType)
                        {
                            case eUserTypes.Admin:
                                redirectionPath = "/AdminHomepage";
                                break;
                        }

                        return RedirectToPage(redirectionPath);
                    }
                }

                ModelState.AddModelError(string.Empty, "Username or Password are incorrect");
                return Page();
            }

            return Page();
        }
    }
}