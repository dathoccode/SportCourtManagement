using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;
using SportCourtManagement.Services.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SportCourtManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly QuanLySanTheThaoContext _context;

        public AdminController(QuanLySanTheThaoContext context)
        {
            _context = context;
        }

        // Quản lý sân
        [HttpGet]
        public async Task<IActionResult> ManageCourts()
        {
            var accountId = HttpContext.Session.GetString("AccountID");
            var roleId = HttpContext.Session.GetString("RoleID");

            if (accountId == null || roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            ViewData["Title"] = "Quản lý sân";

            var courts = await _context.TCourts.ToListAsync();

            return View(courts);
        }

        // Thêm sân
        [HttpGet]
        public async Task<IActionResult> AddCourt()
        {
            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            var sports = await _context.TSports.ToListAsync();
            ViewBag.Sports = new SelectList(sports, "SportId", "SportName");

            var accountId = HttpContext.Session.GetString("AccountID");
            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            ViewData["Title"] = "Thêm sân mới";
            return View(new TCourt()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourt(TCourt model, string OpenTimeString, string CloseTimeString)
        {
            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(OpenTimeString))
            {
                ModelState.AddModelError("OpenTime", "Vui lòng chọn giờ mở cửa.");
            }
            if (string.IsNullOrEmpty(CloseTimeString))
            {
                ModelState.AddModelError("CloseTime", "Vui lòng chọn giờ đóng cửa.");
            }

            ModelState.Remove("Sport");
            ModelState.Remove("CourtId");

            if (ModelState.IsValid)
            {
                try
                {
                    model.OpenTime = TimeOnly.Parse(OpenTimeString);
                    model.CloseTime = TimeOnly.Parse(CloseTimeString);

                    var lastCourt = await _context.TCourts
                                                  .OrderByDescending(c => c.CourtId)
                                                  .FirstOrDefaultAsync();

                    int newIdNum = 1;
                    if (lastCourt != null && lastCourt.CourtId.StartsWith("C"))
                    {
                        string numPart = lastCourt.CourtId.Substring(1); 
                        if (int.TryParse(numPart, out int lastIdNum))
                        {
                            newIdNum = lastIdNum + 1;
                        }
                    }
                    model.CourtId = $"C{newIdNum:D3}"; 

                    model.Rating = Math.Round((new Random().NextDouble() * 5), 1);

                    _context.TCourts.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm sân mới thành công!";
                    return RedirectToAction("ManageCourts");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu: " + ex.Message);
                }
            }

            var sports = await _context.TSports.ToListAsync();
            ViewBag.Sports = new SelectList(sports, "SportId", "SportName", model.SportId);

            var accountId = HttpContext.Session.GetString("AccountID");
            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            ViewData["Title"] = "Thêm sân mới";
            return View(model);
        }

        // Sửa sân
        [HttpGet]
        public async Task<IActionResult> EditCourt(string id) 
        {
            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var court = await _context.TCourts.FindAsync(id);
            if (court == null)
            {
                return NotFound();
            }

            var sports = await _context.TSports.ToListAsync();
            ViewBag.Sports = new SelectList(sports, "SportId", "SportName", court.SportId);

            var accountId = HttpContext.Session.GetString("AccountID");
            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            ViewData["Title"] = "Chỉnh sửa sân";
            return View(court);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourt(string id, TCourt model, string OpenTimeString, string CloseTimeString)
        {
            if (id != model.CourtId)
            {
                return NotFound();
            }

            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(OpenTimeString))
            {
                ModelState.AddModelError("OpenTime", "Vui lòng chọn giờ mở cửa.");
            }
            if (string.IsNullOrEmpty(CloseTimeString))
            {
                ModelState.AddModelError("CloseTime", "Vui lòng chọn giờ đóng cửa.");
            }

            ModelState.Remove("Sport");

            if (ModelState.IsValid)
            {
                try
                {
                    var courtInDb = await _context.TCourts.FindAsync(id);
                    if (courtInDb == null)
                    {
                        return NotFound();
                    }

                    courtInDb.CourtName = model.CourtName;
                    courtInDb.CourtAddress = model.CourtAddress;
                    courtInDb.Contact = model.Contact;
                    courtInDb.SportId = model.SportId;

                    courtInDb.OpenTime = TimeOnly.Parse(OpenTimeString);
                    courtInDb.CloseTime = TimeOnly.Parse(CloseTimeString);

                    _context.Update(courtInDb);
                    await _context.SaveChangesAsync(); 

                    TempData["Success"] = "Cập nhật sân thành công!";
                    return RedirectToAction("ManageCourts");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                }
            }

            var sports = await _context.TSports.ToListAsync();
            ViewBag.Sports = new SelectList(sports, "SportId", "SportName", model.SportId);

            var accountId = HttpContext.Session.GetString("AccountID");
            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            ViewData["Title"] = "Chỉnh sửa sân";
            return View(model);
        }

        // Xoá sân
        [HttpGet]
        public async Task<IActionResult> DeleteCourt(string id)
        {
            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var court = await _context.TCourts.FindAsync(id);
            if (court == null)
            {
                return NotFound();
            }

            var accountId = HttpContext.Session.GetString("AccountID");
            var adminUser = await _context.TAccounts.FindAsync(accountId);
            if (adminUser != null)
            {
                ViewBag.UserName = adminUser.AccName;
                ViewBag.UserAvatar = (adminUser.AccImg != null)
                    ? "data:image/jpeg;base64," + Convert.ToBase64String(adminUser.AccImg)
                    : Url.Content("~/images/userAvatar.jpg");
            }

            // Kiểm tra xem sân này đã có lịch sử đặt sân chưa
            bool hasBookings = await _context.TBookingDetails.AnyAsync(bd => bd.CourtId == id);
            ViewBag.HasBookings = hasBookings; 

            ViewData["Title"] = "Xác nhận xóa sân";
            return View(court);
        }

        [HttpPost, ActionName("DeleteCourt")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourtConfirmed(string id) 
        {
            var roleId = HttpContext.Session.GetString("RoleID");
            if (roleId != "R002")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToAction("Login", "Account");
            }

            var court = await _context.TCourts.FindAsync(id);
            if (court == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.TBookingDetails.AnyAsync(bd => bd.CourtId == id);
            if (hasBookings)
            {
                TempData["Error"] = "Không thể xóa sân đã có lịch sử đặt.";
                return RedirectToAction("ManageCourts");
            }

            try
            {
                // Xóa các dữ liệu liên quan
                var prices = _context.TPrices.Where(p => p.CourtId == id);
                _context.TPrices.RemoveRange(prices);

                var slots = _context.TSlots.Where(s => s.CourtId == id);
                _context.TSlots.RemoveRange(slots);

                // Xóa tất cả liên kết yêu thích
                var courtWithFavs = await _context.TCourts
                                                  .Include(c => c.Accounts)
                                                  .FirstOrDefaultAsync(c => c.CourtId == id);
                if (courtWithFavs != null)
                {
                    courtWithFavs.Accounts.Clear(); 
                }

                _context.TCourts.Remove(court);

                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sân thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi xóa sân: " + ex.Message;
            }

            return RedirectToAction("ManageCourts");
        }
    }
}