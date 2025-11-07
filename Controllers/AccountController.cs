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

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AccountID") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string TaiKhoan, string Password)
        {
            if (string.IsNullOrWhiteSpace(TaiKhoan) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ Email và Mật khẩu!";
                return View();
            }

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
            var accountId = HttpContext.Session.GetString("AccountID");
            if (accountId == null)
            {
                return RedirectToAction("Login");
            }

            var userInDb = await _context.TAccounts.FindAsync(accountId);
            if (userInDb == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }

            userInDb.AccName = model.AccName;
            userInDb.Email = model.Email;
            userInDb.Phone = model.Phone;

            userInDb.Gender = model.Gender;           
            userInDb.DateOfBirth = model.DateOfBirth; 
                                                     
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await AvatarFile.CopyToAsync(memoryStream);
                    userInDb.AccImg = memoryStream.ToArray();
                }
            }

            try
            {
                _context.Update(userInDb);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("AccName", userInDb.AccName);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Không thể lưu thay đổi. Email có thể đã bị trùng.");
                return View(model);
            }
            return RedirectToAction("Profile");
        }

        public IActionResult Loading(string target)
        {
            ViewBag.TargetUrl = target;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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

            return RedirectToAction("Loading", "Account", new { target = Url.Action("Login", "Account") });

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MyBookings()
        {
            var accountId = HttpContext.Session.GetString("AccountID");
            if (accountId == null)
                return RedirectToAction("Login");
            var user = _context.TAccounts.Find(accountId);
            if (user != null)
            {
                ViewBag.UserName = user.AccName;
                ViewBag.UserAvatar = (user.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(user.AccImg)
                    : Url.Content("~/images/userAvatar.jpg"); 
            }
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

