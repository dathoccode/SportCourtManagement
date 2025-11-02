using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SportCourtManagement.Models;
using System.Diagnostics;

namespace SportCourtManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLySanTheThaoContext _context;

        public HomeController(QuanLySanTheThaoContext context)
        {
            _context = context; 
        }
        // Trang chủ
        public async Task<IActionResult> Index()
        {
            string curAccountId = HttpContext.Session.GetString("AccountID");

            if (!string.IsNullOrEmpty(curAccountId))
            {
                var user = await _context.TAccounts.FindAsync(curAccountId);
                if (user != null)
                {
                    ViewBag.UserAvatar = user.AccImg != null
                        ? "data:image/jpeg;base64," + Convert.ToBase64String(user.AccImg)
                        : Url.Content("~/images/userAvatar.jpg");
                    ViewBag.UserName = user.AccName;
                }
            }
            
            var courts = await _context.TCourts.ToListAsync();
            return View(courts);
        }
        
        // Thông tin chi tiết sân
        public async Task<IActionResult> GetCourtDetailsPartial(string courtId) 
        {
            if (string.IsNullOrEmpty(courtId))
            {
                return PartialView("_CourtInfoPartial", null);
            }

            var court = await _context.TCourts
                                .FirstOrDefaultAsync(c => c.CourtId == courtId);
            return PartialView("_CourtInfoPartial", court);
        }

        // Thêm hoặc bỏ thích sân
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(string courtId)
        {
            string curAccountId = HttpContext.Session.GetString("AccountID");

            if (string.IsNullOrEmpty(curAccountId))
                return Json(new { success = false, requiresLogin = true, loginUrl = Url.Action("Login", "Account") });

            if (string.IsNullOrEmpty(courtId))
                return Json(new { success = false, error = "CourtID không hợp lệ." });

            var user = await _context.TAccounts
                                     .Include(a => a.Courts)
                                     .FirstOrDefaultAsync(a => a.AccountId == curAccountId);

            var court = await _context.TCourts.FindAsync(courtId);

            if (user == null || court == null)
                return Json(new { success = false, error = "Không tìm thấy tài khoản hoặc sân." });

            bool isFavorited;

            if (user.Courts.Contains(court))
            {
                user.Courts.Remove(court);
                isFavorited = false;
            }
            else
            {
                user.Courts.Add(court);
                isFavorited = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, isFavorited });
        }


        // Danh sách sân đã thích
        [HttpGet]
        public async Task<IActionResult> GetFavoriteCourtIds()
        {
            string curAccountId = HttpContext.Session.GetString("AccountID");

            if (string.IsNullOrEmpty(curAccountId))
            {
                return Json(new { success = false, requiresLogin = true, loginUrl = Url.Action("Login", "Account") });
            }
                var favoriteCourtIds = await _context.TAccounts
                    .Where(a => a.AccountId == curAccountId)
                    .SelectMany(a => a.Courts.Select(c => c.CourtId)) 
                    .ToListAsync();

                return Json(favoriteCourtIds);
        }

        // Action Error: Xử lý lỗi (mặc định)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}