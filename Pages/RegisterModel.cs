using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;

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

                var user = new User
                {
                    Username = RegisterViewModel.Username,
                    Email = RegisterViewModel.Email,
                    Password = RegisterViewModel.Password,
                    PhoneNumber = RegisterViewModel.PhoneNumber,
                    City = RegisterViewModel.City,
                    Country = RegisterViewModel.Country
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