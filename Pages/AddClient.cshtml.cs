using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Welp.Data;
using Welp.Models;


namespace Welp.Pages
{
    public class AddClientModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }

        public AddClientModel(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnGet()
        {
            // Optional: Initialize any necessary data
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userCookie = _httpContextAccessor.HttpContext.Request.Cookies["UserCookie"];


            try
            {
                string[] userParts = userCookie.Split('|');
                string username = userParts[0];
                string password = userParts[1];
                string salt = userParts[2];
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user.UserType == eUserTypes.Admin)
                {
                    _context.CompanyInfos.Add(CompanyInfo);
                    _context.SaveChanges();
                    return RedirectToPage("/AdminHomePage");
                }

                return RedirectToPage("/Index");// Redirect to the homepage or any other page
            }
            catch 
            {
                return RedirectToPage("/Index");
            }

        }
    }
}
