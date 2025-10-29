// using statements cần thiết
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cho ToListAsync(), FirstOrDefaultAsync()
using SportCourtManagement.Data; // !! THAY THẾ NẾU NAMESPACE CỦA DBCONTEXT KHÁC !!
using SportCourtManagement.Data.Models; // !! THAY THẾ BẰNG NAMESPACE CHỨA CÁC MODELS (tCourt,...) CỦA BẠN !!
using SportCourtManagement.Models; // Namespace chứa ErrorViewModel
using System.Diagnostics;

namespace SportCourtManagement.Controllers // !! KIỂM TRA LẠI NAMESPACE NÀY !!
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // Khai báo DbContext (Tên ApplicationDbContext là do lệnh Scaffold tạo ra)
        private readonly ApplicationDbContext _context;

        // Constructor: Inject ILogger và ApplicationDbContext
        // ASP.NET Core sẽ tự động cung cấp các đối tượng này khi HomeController được tạo
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Gán DbContext được inject vào biến _context
        }

        // Action Index: Hiển thị trang chủ với danh sách sân
        public async Task<IActionResult> Index()
        {
            try
            {
                // Truy vấn CSDL để lấy tất cả các sân từ bảng tCourt
                // !! THAY _context.TCourts BẰNG DbSet ĐÚNG TRONG ApplicationDbContext CỦA BẠN !!
                // ToListAsync() thực thi truy vấn một cách bất đồng bộ
                var courts = await _context.TCourts.OrderByDescending(c => c.Rating).ToListAsync();

                // Truyền danh sách 'courts' vào View (Index.cshtml) làm Model
                return View(courts);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có vấn đề khi truy cập CSDL
                _logger.LogError(ex, "Lỗi khi lấy danh sách sân cho trang Index.");
                // Có thể trả về trang lỗi hoặc một View Index rỗng với thông báo lỗi
                return View(new List<TCourt>()); // Trả về danh sách rỗng
                // Hoặc return RedirectToAction("Error");
            }
        }

        // Action GetCourtDetailsPartial: Xử lý AJAX để lấy thông tin chi tiết sân
        // Được gọi bởi JQuery khi người dùng bấm vào thẻ sân
        public async Task<IActionResult> GetCourtDetailsPartial(string courtId) // courtId là string vì trong DB là nvarchar
        {
            // Kiểm tra xem courtId có giá trị không
            if (string.IsNullOrEmpty(courtId))
            {
                // Trả về PartialView rỗng nếu Id không hợp lệ
                _logger.LogWarning("GetCourtDetailsPartial được gọi với courtId rỗng.");
                return PartialView("_CourtInfoPartial", null);
            }

            try
            {
                // Tìm sân trong database dựa vào courtId
                // !! THAY _context.TCourts BẰNG DbSet ĐÚNG CỦA BẠN !!
                var court = await _context.TCourts
                                  .FirstOrDefaultAsync(c => c.CourtId == courtId);

                // Kiểm tra xem có tìm thấy sân không
                if (court == null)
                {
                    _logger.LogWarning($"Không tìm thấy sân với CourtID: {courtId}");
                    // Trả về PartialView rỗng nếu không tìm thấy sân
                    return PartialView("_CourtInfoPartial", null);
                    // Hoặc return NotFound();
                }

                // Trả về Partial View "_CourtInfoPartial.cshtml" 
                // và truyền đối tượng 'court' (dữ liệu sân) vào làm Model
                return PartialView("_CourtInfoPartial", court);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy chi tiết sân cho CourtID: {courtId}");
                // Trả về PartialView rỗng trong trường hợp lỗi CSDL
                return PartialView("_CourtInfoPartial", null);
                // Hoặc return StatusCode(500, "Lỗi server nội bộ"); // Trả về lỗi 500 để JS xử lý
            }
        }

        // Action Privacy: Trang chính sách bảo mật (mặc định)
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