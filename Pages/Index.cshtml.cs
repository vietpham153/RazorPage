using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;

namespace RazorPage.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _mbContext;

    public IndexModel(ILogger<IndexModel> logger,AppDbContext mbContext)
    {
        _mbContext = mbContext;
        _logger = logger;
    }

    public void OnGet()
    {

        var kq = (from db in _mbContext.articles
                 orderby db.Created descending
                 select db).ToList();
        ViewData["posts"] = kq;
    }
}
