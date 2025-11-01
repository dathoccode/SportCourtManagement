using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace SportCourtManagement.Controllers
{
    public class AccountController : Controller
    {
        // ===== TRANG ĐĂNG NHẬP EMAIL =====
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Xử lý đăng nhập bằng email ở đây
            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public IActionResult Profile()        
        {
            return View();
        }

       
        

        // ===== CÁC TRANG LIÊN QUAN =====
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
             return View();
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
