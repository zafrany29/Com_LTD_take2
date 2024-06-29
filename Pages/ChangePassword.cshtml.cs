using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Welp.Data;
using Welp.Models;
using System.Threading.Tasks;

namespace Welp.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ApplicationDbContext _context;

        public ChangePasswordModel(IHttpContextAccessor httpContextAccessor, ILogger<ChangePasswordModel> logger, ApplicationDbContext i_Context)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = i_Context;
        }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var userCookie = _httpContextAccessor.HttpContext.Request.Cookies["UserCookie"];

            if (ModelState.IsValid)
            {
                // If model state is not valid, return the page with validation errors
                if (!string.IsNullOrEmpty(userCookie))
                {
                    string[] userParts = userCookie.Split('|');
                    string username = userParts[0];
                    string password = userParts[1];
                    string salt = userParts[2];

                    bool userExists = await _context.Users.AnyAsync(u => u.Username == username);
                   // string hashedPassword = Hasher.ComputeHmacHash(LoginViewModel.Password, existingUser.Salt);


                    // Check if the hashed password matches the stored password
                  //  if (existingUser.Password == hashedPassword)

                        if (userExists)
                    {
                        if (Hasher.ComputeHmacHash(OldPassword, salt) == password) // correct password
                        {
                            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                            if (user != null)
                            {
                                String Newsalt = Hasher.GenerateSalt(32);
                                user.Password = Hasher.ComputeHmacHash(NewPassword, Newsalt); // Assuming you have a method like ComputeHmacHash in Hasher class
                                user.Salt = Newsalt;
                                await _context.SaveChangesAsync();
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "password is incorrect");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("UserCookie not found or empty.");
                }

                return Page();
            }

            // Process the form submission (change password logic)
            // Example: 
            // bool passwordChanged = _userService.ChangePassword(User.Identity.Name, OldPassword, NewPassword);

            // Redirect to another page after successful password change
            return RedirectToPage("/Homepage");
        }

        private async Task changePassword(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                user.Password = Hasher.ComputeHmacHash(username, Hasher.GenerateSalt(32)); // Assuming you have a method like ComputeHmacHash in Hasher class
                await _context.SaveChangesAsync();
            }
        }
    }
}