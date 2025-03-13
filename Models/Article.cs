using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPage.Models
{
    //[Table("sth")]
    public class Article
    {
        [Key]
        [DisplayName("STT")]
        public int Id { get; set; }
        [StringLength(250,MinimumLength = 5, ErrorMessage ="{0} chua tu {2} toi {1} ky tu")]
        [Required(ErrorMessage ="{0} phai duoc nhap")]
        [Column(TypeName ="nvarchar")]
        [DisplayName("Tieu de")]
        public string Title { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} phai duoc nhap")]
        [DisplayName("Ngay tao")]
        public DateTime Created { get; set; }
        [Column(TypeName = "ntext")]
        [DisplayName("Noi dung")]
        public string? Content { get; set; }
    }
}
