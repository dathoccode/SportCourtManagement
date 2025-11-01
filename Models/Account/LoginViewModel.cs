using System.ComponentModel.DataAnnotations;

namespace SportCourtManagement.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại.")]
        [Display(Name = "Email hoặc số điện thoại")]
        [RegularExpression(
            @"^(\+84|0[3|5|7|8|9])[0-9]{8}$|^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$",
            ErrorMessage = "Email hoặc số điện thoại không hợp lệ.")]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}
