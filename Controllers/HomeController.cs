using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Welp.Models;
using Welp.Data;
using System.Collections.Generic;

namespace Welp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ensure this action name matches the route
        public async Task<IActionResult> Index()
        {
            var companies = await _context.CompanyInfos.ToListAsync();
            var model = new HomepageModel
            {
                Companies = companies
            };
            return View(model); // Ensure this returns the view with the model
        }
    }
}
