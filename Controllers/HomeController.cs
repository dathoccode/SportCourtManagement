using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Data;
using SportCourtManagement.Data.Models;
using SportCourtManagement.Models;
using System.Diagnostics;

namespace SportCourtManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context; 
        }

        //Hiển thị trang chủ với danh sách sân
        public async Task<IActionResult> Index()
        {
            try
            {
                // Truy vấn CSDL để lấy tất cả các sân từ bảng tCourt
                // ToListAsync() thực thi truy vấn một cách bất đồng bộ
                var courts = await _context.TCourts.OrderByDescending(c => c.Rating).ToListAsync();

                // Truyền danh sách 'courts' vào View (Index.cshtml) làm Model
                return View(courts);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        // Action GetCourtDetailsPartial: Xử lý AJAX để lấy thông tin chi tiết sân
        // Được gọi bởi JQuery khi người dùng bấm vào thẻ sân
        public async Task<IActionResult> GetCourtDetailsPartial(string courtId) 
        {
            // Kiểm tra xem courtId có giá trị không
            if (string.IsNullOrEmpty(courtId))
            {
                return PartialView("_CourtInfoPartial", null);
            }

            try
            {
                // Tìm sân trong database dựa vào courtId
                var court = await _context.TCourts
                                  .FirstOrDefaultAsync(c => c.CourtId == courtId);

                // Trả về Partial View "_CourtInfoPartial.cshtml" 
                // và truyền đối tượng 'court' (dữ liệu sân) vào làm Model
                return PartialView("_CourtInfoPartial", court);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy chi tiết sân {courtId}: {ex.Message}");
                // Trả về PartialView rỗng trong trường hợp lỗi CSDL
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