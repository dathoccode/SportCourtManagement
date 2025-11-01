using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại.")]
        [Display(Name = "Email hoặc số điện thoại")]
        
        
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]

        public string Password { get; set; }
    }
}
