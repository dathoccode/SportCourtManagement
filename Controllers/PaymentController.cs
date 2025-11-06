using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;
using SportCourtManagement.Services.Data;
using System.Text.Json;

namespace SportCourtManagement.Controllers
{
    public class PaymentController : Controller
    {
        private readonly QuanLySanTheThaoContext _context;

        public PaymentController(QuanLySanTheThaoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Index(string slotsJson)
        {
            var accountID = HttpContext.Session.GetString("AccountID");

            string newBookingID = $"B{_context.TBookings.Count() + 1:D3}";

            var user = _context.TAccounts.FirstOrDefault(a => a.AccountId == accountID);

            Console.WriteLine("slotsJson " + slotsJson);
            var slots = JsonSerializer.Deserialize<List<SlotDto>>(slotsJson);

            slots.ForEach(s => Console.WriteLine(s.startTime + ", " + s.endTime));

            if (slots == null) {
                throw new Exception("No slot selected yet");
            }
            // Convert StartTime/EndTime sang TimeSpan để dễ so sánh
            var processed = slots
                .Select(s => new
                {
                    s.slotId,
                    s.courtId,
                    Begin = TimeSpan.Parse(s.startTime),
                    End = TimeSpan.Parse(s.endTime)
                })
                .OrderBy(s => s.courtId)
                .ThenBy(s => s.slotId)
                .ThenBy(s => s.Begin)
                .ToList();

            var merged = new List<TBookingDetail>();

            foreach (var group in processed.GroupBy(s => new { s.courtId, s.slotId }))
            {

                TimeSpan? currentStart = null;
                TimeSpan? currentEnd = null;

                foreach (var slot in group)
                {
                    if (currentStart == null)
                    {
                        currentStart = slot.Begin;
                        currentEnd = slot.End;
                    }
                    else if (slot.Begin == currentEnd)
                    {
                        currentEnd = slot.End;
                    }
                    else
                    {
                        merged.Add(new TBookingDetail
                        {
                            BookingId = newBookingID,
                            DetailId = $"B{_context.TBookingDetails.Count() + 1:D3}",
                            CourtId = group.Key.courtId,
                            SlotId = group.Key.slotId,
                            StartTime = TimeOnly.FromTimeSpan(currentStart.Value),
                            EndTime = TimeOnly.FromTimeSpan(currentEnd.Value)
                        });

                        // bắt đầu chuỗi mới
                        currentStart = slot.Begin;
                        currentEnd = slot.End;
                    }
                }

                // lưu bản ghi cuối cùng trong nhóm
                if (currentStart != null)
                {
                    merged.Add(new TBookingDetail
                    {
                        DetailId = Guid.NewGuid().ToString(),
                        CourtId = group.Key.courtId,
                        SlotId = group.Key.slotId,
                        StartTime = TimeOnly.FromTimeSpan(currentStart.Value),
                        EndTime = TimeOnly.FromTimeSpan(currentEnd.Value)
                    });
                }
            }

            ViewBag.CourtInf = _context.TCourts
                .Where(c => c.CourtId == merged[0].CourtId)
                .Select(c => new
                {
                    c.CourtName,
                    c.CourtAddress
                }).ToList();
            ViewBag.UserName = user.AccName;
            ViewBag.UserPhone = user.Phone;
            ViewBag.TotalTime = slots.Count() / 2f;

            _context.TBookings.Add(new TBooking
            {
                BookingId = newBookingID,
                AccountId = accountID,
                BookingDate = DateTime.Now,
                Sale = 0,
                Price = slots.Count() * 100000
            });
            _context.TBookingDetails.AddRange(merged);
            _context.SaveChangesAsync();

            return View(merged);
        }

        public IActionResult Success(string BookingId)
        {
            return View();
        }
    }
}
