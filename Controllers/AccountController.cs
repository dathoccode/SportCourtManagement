using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportCourtManagement.Models; // Namespace chứa QuanLySanTheThaoContext
using System.Linq;

namespace SportCourtManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly QuanLySanTheThaoContext _context;

        public AccountController(QuanLySanTheThaoContext context)
        {
            _context = context;
        }

        // ======= [GET] TRANG ĐĂNG NHẬP =======
        [HttpGet]
        public IActionResult Login()
        {
            // Nếu đã đăng nhập rồi → chuyển về trang chủ
            if (HttpContext.Session.GetString("AccountID") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ======= [POST] XỬ LÝ ĐĂNG NHẬP =======
        [HttpPost]
        public IActionResult Login(string TaiKhoan, string Password)
        {
            if (string.IsNullOrWhiteSpace(TaiKhoan) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ Email và Mật khẩu!";
                return View();
            }

            // Tìm tài khoản theo email hoặc số điện thoại
            var acc = _context.TAccounts
                .FirstOrDefault(a => (a.Email == TaiKhoan || a.Phone == TaiKhoan)
                                  && a.AccPassword == Password);

            if (acc == null)
            {
                ViewBag.Error = "Sai Email/SĐT hoặc Mật khẩu!";
                return View();
            }

            // Lưu thông tin đăng nhập vào Session
            HttpContext.Session.SetString("AccountID", acc.AccountId);
            HttpContext.Session.SetString("AccName", acc.AccName ?? "");
            HttpContext.Session.SetString("RoleID", acc.RoleId?.ToString() ?? "0");
            HttpContext.Session.SetString("Email", acc.Email ?? "");

            // Chuyển hướng sau đăng nhập thành công
            // Sau khi đăng nhập thành công
            return RedirectToAction("Loading", "Account", new { target = Url.Action("Index", "Home") });

        }

        // ======= [GET] TRANG HỒ SƠ CÁ NHÂN =======
        [HttpGet]
        public IActionResult Profile()
        {
            var accountId = HttpContext.Session.GetString("AccountID");
            if (accountId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập trước!";
                return RedirectToAction("Login");
            }


            var acc = _context.TAccounts.FirstOrDefault(a => a.AccountId == accountId);
            if (acc == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin tài khoản!";
                return RedirectToAction("Login");
            }

            return View(acc);
        }
        public IActionResult Loading(string target)
        {
            ViewBag.TargetUrl = target;
            return View();
        }

        // ======= [GET] TRANG ĐĂNG KÝ =======
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }

        // ======= [POST] XỬ LÝ ĐĂNG KÝ =======
        [HttpPost]
        
        public IActionResult Register(string FullName, string email, string phone, string password)
        {
           
            bool exists = _context.TAccounts.Any(a => a.Email == email || a.Phone == phone);
            if (exists)
            {
                ViewBag.Error = "Email hoặc số điện thoại đã tồn tại!";
                return View();
            }

            
            string newId = $"ACC{_context.TAccounts.Count() + 1:D3}";

            var acc = new TAccount
            {
                AccountId = newId,
                RoleId = 1, 
                AccName = FullName,
                Email = email,
                Phone = phone,
                AccPassword = password
            };

            _context.TAccounts.Add(acc);
            _context.SaveChanges();


            // Sau khi đăng ký thành công
            return RedirectToAction("Loading", "Account", new { target = Url.Action("Login", "Account") });

        }

        // ======= [GET] QUÊN MẬT KHẨU =======
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // ======= [GET] DANH SÁCH LỊCH ĐẶT =======
        [HttpGet]
        public IActionResult MyBookings()
        {
            var accountId = HttpContext.Session.GetString("AccountID");
            if (accountId == null)
                return RedirectToAction("Login");

            ViewData["Title"] = "Lịch đã đặt";
            var bookings = _context.TBookings
                                   .Where(b => b.AccountId == accountId)
                                   .OrderByDescending(b => b.BookingDate)
                                   .ToList();

            return View(bookings);
        }

        // ======= ĐĂNG XUẤT =======
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
