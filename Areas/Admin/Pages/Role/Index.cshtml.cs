using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using System.Threading.Tasks;

namespace RazorPage.Areas.Admin.Pages.Role
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, AppDbContext myBlogContext) : base(roleManager, myBlogContext)
        {
            
        }

        public class RoleModel : IdentityRole
        {
            public string[] Claims { get; set; }
        }

        public List<RoleModel> roles { get; set; }

        public async Task OnGet()
        {
            var r = await _roleManager.Roles.OrderBy(r=>r.Name).ToListAsync();
            roles = new List<RoleModel>();
            foreach(var item in r)
            {
                var claim = await _roleManager.GetClaimsAsync(item);
                var claimString = claim.Select(c => c.Type + "="+c.Value);
                var rm = new RoleModel
                {
                    Name = item.Name,
                    Id = item.Id,
                    Claims = claimString.ToArray()
                };
                roles.Add(rm);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
