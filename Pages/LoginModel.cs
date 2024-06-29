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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(ApplicationDbContext i_Context, IHttpContextAccessor i_HttpContextAccessor)
        {
            _context = i_Context;
            _httpContextAccessor = i_HttpContextAccessor;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();

        public void OnGet()
        {
            // Clear previous cookies
            ClearCookies();

            // Initialize with default values if needed
            LoginViewModel = new LoginViewModel(); // Ensure the view model is not null
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear previous cookies
            ClearCookies();

            if (ModelState.IsValid)
            {
                // Retrieve the user by username
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == LoginViewModel.Username);

                if (existingUser != null)
                {
                    // Hash the input password with the user's salt
                    string hashedPassword = Hasher.ComputeHmacHash(LoginViewModel.Password, existingUser.Salt);

                    // Check if the hashed password matches the stored password
                    if (existingUser.Password == hashedPassword)
                    {
                        // Create new cookies
                        createCookie(existingUser);

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

        private void createCookie(Models.User i_User)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                IsEssential = true,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserCookie", $"{i_User.Username}|{i_User.Password}|{i_User.Salt}", cookieOptions);
        }

        private void ClearCookies()
        {
            foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie);
            }
        }
    }
}
