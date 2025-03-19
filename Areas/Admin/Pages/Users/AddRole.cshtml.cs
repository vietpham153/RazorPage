// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _myBlogContext;

        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext myBlogContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _myBlogContext = myBlogContext;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        [DisplayName("Danh sách các vai trò")]
        public string[] roleName { get; set; }



        public AppUser user { get; set; }
        public SelectList allRoles { get; set; }
        public List<IdentityRoleClaim<string>> claimsInRole { get; set; }
        public List<IdentityUserClaim<string>> claimsInUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return NotFound($"Không có người dùng");
            }    
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy người dùng với id = {id}");
            }

            roleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            await GetClaimsAsync(id);

            return Page();
        }
        async Task GetClaimsAsync(string id)
        {
            var listRole = from r in _myBlogContext.Roles
                           join ur in _myBlogContext.UserRoles on r.Id equals ur.RoleId
                           where ur.UserId == id
                           select r;

            var _claimsInRole = from c in _myBlogContext.RoleClaims
                                join r in listRole on c.RoleId equals r.Id
                                select c;

            claimsInRole = await _claimsInRole.ToListAsync();

            claimsInUser = await (from c in _myBlogContext.UserClaims
                         where c.UserId == id
                         select c).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound($"Không có người dùng");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy người dùng với id = {id}");
            }

            await GetClaimsAsync(id);

            var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray();

            var deleteRoles = oldRoleName.Where(r => !roleName.Contains(r));
            var addRoles = roleName.Where(r => !oldRoleName.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            allRoles = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(errors =>
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                });
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(errors =>
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                });
                return Page();
            }

            StatusMessage = $"Đã cập nhật thành công vai trò cho {user.UserName}.";

            return RedirectToPage("./Index");
        }
    }
}
