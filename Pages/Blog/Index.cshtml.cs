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
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RazorPage.Models.AppDbContext _context;
        public const int ITEMS_PER_PAGE = 10;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPages { get; set; }
        public int countPages { get; set; }
        public IndexModel(RazorPage.Models.AppDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            int totalArticle = _context.articles.Count();
            countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);

            if (currentPages < 1) 
                currentPages = 1;
            if (currentPages > countPages)
                currentPages = countPages;

            var kq = (from a in _context.articles
                     orderby a.Created descending
                     select a)
                     .Skip((currentPages - 1) * ITEMS_PER_PAGE)
                     .Take(ITEMS_PER_PAGE);

            if (!string.IsNullOrEmpty(SearchString))
            {
                Article = await kq.Where(p => p.Title.Contains(SearchString)).ToListAsync();
            }
            else
            {
                Article = await kq.ToListAsync();
            }
                //Article = await _context.articles.ToListAsync();
                
        }
    }
}
