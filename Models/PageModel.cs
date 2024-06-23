using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Welp.Data;
using Welp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Welp.Pages
{
    public class HomepageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public HomepageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CompanyInfo> Companies { get; set; }

        public async Task OnGetAsync()
        {
            Companies = await _context.CompanyInfos.ToListAsync();
        }
    }
}
