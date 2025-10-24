using Microsoft.AspNetCore.Mvc;

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

        // ===== TRANG ĐĂNG NHẬP BẰNG SỐ ĐIỆN THOẠI =====
        [HttpGet]
        public IActionResult LoginByPhone()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginByPhone(string phone, string password)
        {
            // TODO: Xử lý đăng nhập bằng số điện thoại ở đây
            if (phone == "0123456789" && password == "123456")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Số điện thoại hoặc mật khẩu không đúng!";
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
    }
}
