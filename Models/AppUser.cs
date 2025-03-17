using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPage.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string? HomeAdress { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DoB { get; set; }
    }
}
