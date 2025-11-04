using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;
using SportCourtManagement.Services.Data;
using System.Security.Claims;

namespace SportCourtManagement.Controllers
{
    public class BookingController : Controller
    {
        private readonly QuanLySanTheThaoContext _context;

        public BookingController(QuanLySanTheThaoContext context)
        {
            _context = context;
        }

        // GET: Booking1/Create
        public IActionResult Create(string id)
        {
            // Lấy thông tin tài khoản đang đăng nhập
            var userEmail = User.Identity?.Name; // nếu đăng nhập bằng email
            var user = _context.TAccounts.FirstOrDefault(a => a.Email == userEmail);

            // Lấy thông tin booking có sẵn trong DB
            var booking = _context.TBookings
                .Include(b => b.TBookingDetails)
                    .ThenInclude(d => d.Court)
                .Include(b => b.Status)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            // Gửi thông tin tài khoản vào ViewBag
            ViewBag.CustomerName = user?.AccName ?? "Khách hàng";
            ViewBag.PhoneNumber = user?.Phone ?? "Chưa có số điện thoại";

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(string BookingId, string CustomerName, string PhoneNumber, string Note)
        {
            // Tìm booking theo ID
            var booking = _context.TBookings
                .Include(b => b.TBookingDetails)
                    .ThenInclude(d => d.Court)
                .Include(b => b.Status)
                .FirstOrDefault(b => b.BookingId == BookingId);

            if (booking == null)
                return NotFound();

            // Lưu tạm thông tin người dùng (chưa cần ghi DB)
            ViewBag.CustomerName = CustomerName;
            ViewBag.PhoneNumber = PhoneNumber;
            ViewBag.Note = Note;

            // Trả về view Confirm.cshtml
            return View("Confirm", booking);
        }

        // GET: Booking1/Success
        public IActionResult Success(string BookingId)
        {
            var booking = _context.TBookings
                .Include(b => b.TBookingDetails)
                    .ThenInclude(d => d.Court)
                .Include(b => b.Status)
                .FirstOrDefault(b => b.BookingId == BookingId);

            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}
