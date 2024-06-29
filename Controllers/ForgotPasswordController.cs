using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Welp.Data;
using Welp.Models;

namespace Welp.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailjetClient _mailjetClient;

        public ForgotPasswordController(ApplicationDbContext context, IMailjetClient mailjetClient)
        {
            _context = context;
            _mailjetClient = mailjetClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ForgotPasswordViewModel
            {
                Username = string.Empty,
            };
            ViewData["Title"] = "Forgot Password";
            return View("~/Pages/Forgot_Password.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SendResetLink(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user != null)
                {
                    // Generate new random password
                    var newPassword = GenerateRandomPassword();

                    // Generate new salt
                    var newSalt = GenerateSalt(32);

                    // Compute HMAC hash with the new salt
                    var hashedPassword = ComputeHmacHash(newPassword, newSalt);

                    // Update user password and salt
                    user.Password = hashedPassword;
                    user.Salt = newSalt;
                    await _context.SaveChangesAsync();

                    // Send email with new password
                    var emailSent = await SendPasswordEmail(user.Email, newPassword);
                    if (emailSent)
                    {
                        return RedirectToPage("/PasswordSent");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to send email.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User does not exist, please try again.");
                    ViewData["Title"] = "Forgot Password";
                    return View("~/Pages/Forgot_Password.cshtml", model);
                }
            }
            ViewData["Title"] = "Forgot Password";
            return View("~/Pages/Forgot_Password.cshtml", model);
        }

        private string GenerateRandomPassword(int length = 10)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var password = new string(Enumerable.Repeat(validChars, length)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

        private string GenerateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[size];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
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

        private async Task<bool> SendPasswordEmail(string email, string newPassword)
        {
            var request = new MailjetRequest
            {
                Resource = Send.Resource
            }
            .Property(Send.FromEmail, "omerzafrany123@gmail.com")
            .Property(Send.FromName, "Comunication_LTD")
            .Property(Send.Subject, "Your New Password")
            .Property(Send.TextPart, $"Your new password is: {newPassword}")
            .Property(Send.Recipients, new JArray {
                new JObject {
                    { "Email", email }
                }
            });

            var response = await _mailjetClient.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}