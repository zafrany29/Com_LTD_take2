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
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();

        public void OnGet()
        {
            // Initialize with default values if needed
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == RegisterViewModel.Username || u.Email == RegisterViewModel.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "User already exists, please try again.");
                    return Page();
                }

                // Generate salt
                var salt = Hasher.GenerateSalt(32);

                // Compute HMAC hash
                var hashedPassword = Hasher.ComputeHmacHash(RegisterViewModel.Password, salt);

                var user = new User
                {
                    Username = RegisterViewModel.Username,
                    Email = RegisterViewModel.Email,
                    Password = hashedPassword,
                    PhoneNumber = RegisterViewModel.PhoneNumber,
                    City = RegisterViewModel.City,
                    Country = RegisterViewModel.Country,
                    UserType = RegisterViewModel.UserType,
                    Salt = salt
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                // Redirect to "Homepage" upon successful registration
                return RedirectToPage("/Homepage");
            }

            return Page();
        }
    }
}