using Microsoft.AspNetCore.Mvc;
using SportCourtManagement.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace SportCourtManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Profile()
        {
            // === PHẦN DỮ LIỆU MẪU (FAKE DATA) ===
            // Bình thường ta sẽ lấy dữ liệu từ database
            // Nhưng giờ ta chỉ làm front-end, nên ta tự "bịa" ra:

            var fakeUserData = new UserProfileViewModel
            {
                FullName = "Nguyễn Văn A",
                Email = "nguyenvana@gmail.com",
                PhoneNumber = "0987654321",
                ProfileImageUrl = "/images/acma.jpg", // Link ảnh mẫu
                Gender = "Nam",
                DateOfBirth = new DateTime(2000, 5, 20)
            };

            // Đưa dữ liệu mẫu này ra ngoài trang View
            return View(fakeUserData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(UserProfileViewModel model)
        {
            // Bỏ qua ModelState.IsValid như bạn yêu cầu

            // Bắt đầu logic lưu file
            if (model.AvatarFile != null)
            {
                // 1. Lấy đường dẫn wwwroot
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // 2. Tạo tên file ngẫu nhiên, duy nhất
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarFile.FileName);

                // 3. Tạo đường dẫn đầy đủ để lưu file
                string imagePath = Path.Combine(wwwRootPath, "images", fileName);

                // 4. Lưu file vào đường dẫn đó
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    model.AvatarFile.CopyTo(fileStream);
                }

                // 5. Lấy đường dẫn tương đối để lưu vào DB (sẽ dùng khi có DB)
                // string newAvatarPath = "/images/" + fileName;
            }

            // Thông báo thành công
            //TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";

            // Chuyển hướng
            return RedirectToAction(nameof(Profile));
        }
        [HttpGet]
        public IActionResult MyBookings()
        {
            ViewData["Title"] = "Lịch đã đặt";
            // Tạm thời chỉ trả về View. Sau này sẽ truyền dữ liệu lịch đặt vào đây.
            return View();
        }
    }
}
