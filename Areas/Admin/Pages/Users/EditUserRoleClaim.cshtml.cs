using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;

namespace RazorPage.Areas.Admin.Pages.Users
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly MyBlogContext _myBlogContext;
        private readonly UserManager<AppUser> _userManager;
        public EditUserRoleClaimModel(MyBlogContext myBlogContext, UserManager<AppUser> userManager)
        {
            _myBlogContext = myBlogContext;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public NotFoundObjectResult OnGet()
        {
            return NotFound("Không được truy cập");
        }
        public class InputModel()
        {
            [DisplayName("Tên tính chất")]
            [Required(ErrorMessage = "Vui lòng nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} cho tới {1} ký tự.")]
            public string ClaimType { get; set; }
            [DisplayName("Giá trị tính chất")]
            [Required(ErrorMessage = "Vui lòng nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} cho tới {1} ký tự.")]
            public string ClaimValue { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public AppUser user { get; set; }
        public IdentityUserClaim<string> userRoleClaim { get; set; }

        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null) { return NotFound("Không tìm thất user"); }
            return Page();
        }
        public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null) { return NotFound("Không tìm thất user"); }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var claims = _myBlogContext.UserClaims.Where(uc => uc.UserId == user.Id);

            if(claims.Any(uc => uc.ClaimType == Input.ClaimType && uc.ClaimValue == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có");
                return Page();
            }

            await _userManager.AddClaimAsync(user, new Claim(Input.ClaimType, Input.ClaimValue));
            StatusMessage = "Đã thêm đặc tính cho User";

            return RedirectToPage("./AddRole", new {Id = Url});
        }
        public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            if (claimid == null) { return NotFound("Không tìm thất user"); }

            userRoleClaim = _myBlogContext.UserClaims.Where(c=>c.Id == claimid).FirstOrDefault();

            user = await _userManager.FindByIdAsync(userRoleClaim.UserId);
            if (user == null) { return NotFound("Không tìm thất user"); }

            Input = new InputModel()
            {
                ClaimType = userRoleClaim.ClaimType,
                ClaimValue = userRoleClaim.ClaimValue
            };

            return Page();
        }
        public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {
            if (claimid == null) { return NotFound("Không tìm thất user"); }

            userRoleClaim = _myBlogContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();

            user = await _userManager.FindByIdAsync(userRoleClaim.UserId);
            if (user == null) { return NotFound("Không tìm thất user"); }
            if (!ModelState.IsValid) return Page();

            if(_myBlogContext.UserClaims.Any(uc => uc.UserId == user.Id && uc.ClaimType == Input.ClaimType && uc.ClaimValue == Input.ClaimValue && uc.Id != userRoleClaim.Id))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có");
                return Page();
            }    
            userRoleClaim.ClaimValue = Input.ClaimValue;
            userRoleClaim.ClaimType = Input.ClaimType;

            await _myBlogContext.SaveChangesAsync();
            StatusMessage = "Bạn đã cập nhật thành công";

            return RedirectToPage("./AddRole", new { Id = user.Id });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null) { return NotFound("Không tìm thất user"); }

            userRoleClaim = _myBlogContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();

            user = await _userManager.FindByIdAsync(userRoleClaim.UserId);
            if (user == null) { return NotFound("Không tìm thất user"); }
            await _userManager.RemoveClaimAsync(user, new Claim(userRoleClaim.ClaimType, userRoleClaim.ClaimValue);
            StatusMessage = "Bạn đã xóa thuộc tính thành công";

            return RedirectToPage("./AddRole", new { Id = user.Id });
        }
    }
}
