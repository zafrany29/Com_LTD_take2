using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Welp.Data;
using Welp.Models;

namespace Welp.Pages
{
    public class AddClientModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public CompanyInfo CompanyInfo { get; set; }

        public AddClientModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Optional: Initialize any necessary data
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CompanyInfos.Add(CompanyInfo);
            _context.SaveChanges();

            return RedirectToPage("/AdminHomePage"); // Redirect to the homepage or any other page
        }
    }
}
