using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Pages_Blog
{
    [Authorize(Policy ="Genz")] // Cho nguoi dung co DoB 1997-2012 truy cap
    public class DetailsModel : PageModel
    {
        private readonly RazorPage.Models.AppDbContext _context;

        public DetailsModel(RazorPage.Models.AppDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);

            if (article is not null)
            {
                Article = article;

                return Page();
            }

            return NotFound();
        }
    }
}
