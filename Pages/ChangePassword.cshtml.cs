using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Welp.Data;
using Welp.Models;
using System.Text.Json;
using System.Collections.Generic;

namespace Welp.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ApplicationDbContext _context;

        public ChangePasswordModel(IHttpContextAccessor httpContextAccessor, ILogger<ChangePasswordModel> logger, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
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
                    string storedPasswordHash = userParts[1];
                    string salt = userParts[2];

                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                    if (user != null && Hasher.ComputeHmacHash(OldPassword, salt) == storedPasswordHash) // correct password
                    {
                        // Verify that the new password is not in the history
                        var passwordHistory = JsonSerializer.Deserialize<List<string>>(user.PasswordHistory ?? "[]");
                        string newHashedPassword = Hasher.ComputeHmacHash(NewPassword, salt);

                        if (passwordHistory.Contains(newHashedPassword))
                        {
                            ModelState.AddModelError(string.Empty, "Cannot reuse the last 3 passwords.");
                            return Page();
                        }

                        // Update the password and the history
                        user.Password = Hasher.ComputeHmacHash(NewPassword, salt);
                        passwordHistory.Add(newHashedPassword);

                        // Keep only the last 3 passwords
                        if (passwordHistory.Count > 3)
                        {
                            passwordHistory.RemoveAt(0);
                        }

                        user.PasswordHistory = JsonSerializer.Serialize(passwordHistory);

                        _context.Update(user);
                        await _context.SaveChangesAsync();

                        // Update the cookie with the new hashed password and salt
                        var newCookieValue = $"{username}|{user.Password}|{salt}";
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("UserCookie", newCookieValue);

                        return RedirectToPage("/Homepage");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password is incorrect");
                    }
                }
                else
                {
                    _logger.LogInformation("UserCookie not found or empty.");
                }
            }

            // Process the form submission (change password logic)
            return Page();
        }
    }

    public class ChangePasswordViewModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}