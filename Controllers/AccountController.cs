using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace SportCourtManagement.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Profile()
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
