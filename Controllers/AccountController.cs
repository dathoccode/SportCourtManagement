using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;
using SportCourtManagement.Services.Data;
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
            HttpContext.Session.SetString("RoleID", acc.RoleId?.ToString() ?? "R001");
            HttpContext.Session.SetString("Email", acc.Email ?? "");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(TAccount model, IFormFile? AvatarFile)
        {
            // 1. Lấy ID user từ session
            var accountId = HttpContext.Session.GetString("AccountID");
            if (accountId == null)
            {
                return RedirectToAction("Login");
            }

            // 2. Tìm user gốc trong CSDL
            var userInDb = await _context.TAccounts.FindAsync(accountId);
            if (userInDb == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            // 3. Cập nhật thông tin từ form (biến "model") vào user CSDL
            // ASP.NET đã tự động điền thông tin từ form vào biến "model"
            userInDb.AccName = model.AccName;
            userInDb.Email = model.Email;
            userInDb.Phone = model.Phone;

            // === ĐÂY LÀ 2 DÒNG BẠN HỎI ===
            userInDb.Gender = model.Gender;           // Cập nhật giới tính
            userInDb.DateOfBirth = model.DateOfBirth; // Cập nhật ngày sinh
                                                      // =============================

            // 4. Xử lý ảnh đại diện (nếu user có tải file mới)
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                // Chuyển file ảnh thành dạng byte[] để lưu vào CSDL
                using (var memoryStream = new MemoryStream())
                {
                    await AvatarFile.CopyToAsync(memoryStream);
                    userInDb.AccImg = memoryStream.ToArray();
                }
            }

            // 5. Lưu tất cả thay đổi vào Database
            try
            {
                _context.Update(userInDb); // Báo cho EF biết là đối tượng này đã bị thay đổi
                await _context.SaveChangesAsync(); // Thực thi lệnh UPDATE

                // Cập nhật lại tên trong Session phòng khi user đổi tên
                HttpContext.Session.SetString("AccName", userInDb.AccName);
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi (ví dụ: email bị trùng)
                ModelState.AddModelError("", "Không thể lưu thay đổi. Email có thể đã bị trùng.");
                return View(model); // Trả về trang Profile và hiển thị lỗi
            }

            // 6. Quay về trang Profile sau khi lưu thành công
            return RedirectToAction("Profile");
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
                RoleId = "R001", 
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
            // ===== BẮT ĐẦU CODE THÊM MỚI =====
            var user = _context.TAccounts.Find(accountId);
            if (user != null)
            {
                ViewBag.UserName = user.AccName;
                ViewBag.UserAvatar = (user.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(user.AccImg)
                    : Url.Content("~/images/userAvatar.jpg"); // Ảnh đại diện mặc định
            }
            // ===== KẾT THÚC CODE THÊM MỚI =====
            ViewData["Title"] = "Lịch đã đặt";
            var bookings = _context.TBookings
            .Include(b => b.Status) 
            .Include(b => b.TBookingDetails) 
                .ThenInclude(bd => bd.Court) 
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

