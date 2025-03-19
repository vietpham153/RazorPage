using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, AppDbContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }
        public IdentityRole role { get; set; }
        public async Task<IActionResult> OnGet(string routeid)
        {
            if (routeid == null) return NotFound("Không tìm thấy vai trò");
            role = await _roleManager.FindByIdAsync(routeid);
            if (role == null) return NotFound("Không tìm thấy vai trò");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string routeid)
        {
            if (!ModelState.IsValid) return NotFound("Không tìm thấy vai trò");
            role = await _roleManager.FindByIdAsync(routeid);
            if (role == null) return NotFound("Không tìm thất vai trò");
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) 
            {
                StatusMessage = $"Xóa vai trò {role} thành công";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(erors =>
                {
                    ModelState.AddModelError(string.Empty, erors.Description);
                });
                return Page();
            }

        }
    }
}
