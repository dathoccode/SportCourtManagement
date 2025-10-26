using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace SportCourtManagement.Models
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }

        public string Gender { get; set; } // Sẽ là "Nam", "Nữ", "Khác"

        [DataType(DataType.Date)] // Giúp hiển thị trình chọn ngày
        public DateTime? DateOfBirth { get; set; } // Dùng DateTime? (có dấu ?)
        
        [Display(Name = "Ảnh đại diện mới")]
        public IFormFile AvatarFile { get; set; }
    }
}
