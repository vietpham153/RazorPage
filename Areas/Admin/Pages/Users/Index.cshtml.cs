using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using System.Threading.Tasks;

namespace RazorPage.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;   
        }
        public const int ITEMS_PER_PAGE = 10;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPages { get; set; }
        public int countPages { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class UserRole : AppUser
        {
            public string roleName { get; set; }
        }
        public List<UserRole> users { get; set; }
        public int totalUsers { get; set; }
        public async Task OnGet()
        {
            var qr = _userManager.Users.OrderBy(u => u.UserName);
            //users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            totalUsers = await qr.CountAsync();
            countPages = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);

            if (currentPages < 1)
                currentPages = 1;
            if (currentPages > countPages)
                currentPages = countPages;

            var qr1 = qr.Skip((currentPages - 1) * ITEMS_PER_PAGE)
                        .Take(ITEMS_PER_PAGE)
                        .Select(u=>new UserRole
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                        });
            users = await qr1.ToListAsync(); 

            foreach(var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                user.roleName = string.Join(",", role);
            }
        }
    }
}
