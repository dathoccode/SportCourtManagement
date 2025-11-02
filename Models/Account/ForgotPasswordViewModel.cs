using System.ComponentModel.DataAnnotations;
namespace SportCourtManagement.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại")]
        [Display(Name = "Số điện thoại hoặc Email")]
        public string phoneOrEmail { get; set; }
    }
}
