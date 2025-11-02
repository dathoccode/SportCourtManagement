using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var courts = await _context.TCourts.OrderByDescending(c => c.Rating).ToListAsync();
                return View(courts);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> GetCourtDetailsPartial(string courtId) 
        {
            if (string.IsNullOrEmpty(courtId))
            {
                return PartialView("_CourtInfoPartial", null);
            }

            try
            {
                var court = await _context.TCourts
                                  .FirstOrDefaultAsync(c => c.CourtId == courtId);
                return PartialView("_CourtInfoPartial", court);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy chi tiết sân {courtId}: {ex.Message}");
                return PartialView("_CourtInfoPartial", null);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Action Error: Xử lý lỗi (mặc định)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}