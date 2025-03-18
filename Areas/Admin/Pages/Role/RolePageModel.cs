﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;

namespace RazorPage.Areas.Admin.Pages.Role
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly MyBlogContext _myBlogContext;
        [TempData]
        public string StatusMessage { get; set; }
        public RolePageModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext)
        {
            _roleManager = roleManager;
            _myBlogContext = myBlogContext;
        }
    }
}
