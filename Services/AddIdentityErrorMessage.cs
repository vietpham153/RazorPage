using Microsoft.AspNetCore.Identity;

namespace RazorPage.Services
{
    public class AddIdentityErrorMessage : IdentityErrorDescriber
    {
        public override IdentityError DuplicateRoleName(string role)
        {
            var er = base.DuplicateRoleName(role);
            return new IdentityError
            {
                Code = er.Code,
                Description = $"Tên Role bị trùng: {role}",
            };
        }
    }
}
