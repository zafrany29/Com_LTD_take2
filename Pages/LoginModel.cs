using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using Mailjet.Client.Resources;

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

        private string ComputeSha1Hash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha1.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Hash the input password
                string hashedPassword = ComputeSha1Hash(LoginViewModel.Password);

                // Check if the user exists with the hashed password
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == LoginViewModel.Username && u.Password == hashedPassword);

                if (existingUser != null)
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password are incorrect");
                    return Page();
                }
            }

            return Page();
        }
    }
}