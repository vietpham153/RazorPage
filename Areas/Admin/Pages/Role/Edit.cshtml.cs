using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPage.Areas.Admin.Pages.Role
{
    [Authorize(Policy = "AllowEditRole")]
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public class InputModel
        {
            [DisplayName("Tên vai trò")]
            [Required(ErrorMessage ="Vui lòng nhập {0}")]
            [StringLength(256,MinimumLength = 3, ErrorMessage ="{0} phải có chiều dài tối thiểu là {2} và tối đa {1} ký tự.")]
            public string Name { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public List<IdentityRoleClaim<string>> Claims { get; set; }
        public IdentityRole role { get; set; }

        public async Task<IActionResult> OnGet(string routeid)
        {
            if(routeid == null)
            {
                return NotFound("Không tìm thấy role.");
            }
            role = await _roleManager.FindByIdAsync(routeid);
            if(role != null)
            {
                Input = new InputModel
                {
                    Name = role.Name
                };
                Claims = await _myBlogContext.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
                return Page();
            }
            return NotFound("Không tìm thấy role.");
        }
        public async Task<IActionResult> OnPostAsync(string routeid)
        {
            if(routeid == null)
            {
                return NotFound("Không tìm thấy vai trò");
            }
            role = await _roleManager.FindByIdAsync(routeid);
            if (role == null) 
            {
                return NotFound("Không tìm thấy vai trò");
            }
            Claims = await _myBlogContext.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existRole = await _roleManager.FindByNameAsync(Input.Name);
            if (existRole != null)
            {
                ModelState.AddModelError(string.Empty, "Không được trùng tên với Role cũ");
                return Page();
            }
            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn đã cập nhật vai trò mới: {Input.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();
        }
    }
}
