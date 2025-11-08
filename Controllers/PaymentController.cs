using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models;
using SportCourtManagement.Services.Data;
using SportCourtManagement.ViewModels.Payment;
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
            var viewModel = new BookingInfoViewModel();

            var accountID = HttpContext.Session.GetString("AccountID");
            viewModel.account = _context.TAccounts
                .Where(a => a.AccountId == accountID)
                .FirstOrDefault();

            var newBookingID = $"B{_context.TBookings.Count() + 1:D3}";

            var newBooking = new TBooking
            {
                BookingId = $"B{_context.TBookings.Count() + 1:D3}",
                AccountId = accountID,
                BookingDate = DateTime.Now,
                Sale = 0,
                StatusId = "STT002",
                Price = 0 // sẽ cập nhật sau khi tính toán chi tiết booking
            };

            //thông tin booking
            viewModel.booking = newBooking;


            // lấy thông tin booking được gửi lên từ form
            var slots = JsonSerializer.Deserialize<List<SlotDto>>(slotsJson);
            if(slots == null)
            {
                throw new Exception("No slot selected yet");
            }

            // Convert StartTime/EndTime sang TimeSpan và sắp xếp theo thời gian bắt đầu để dễ so sánh
            var processed = slots
                .Select(s => new
                {
                    s.slotId,
                    s.courtId,
                    Begin = s.startTime,
                    End = s.endTime
                })
                .OrderBy(s => s.courtId)
                .ThenBy(s => s.slotId)
                .ThenBy(s => s.Begin)
                .ToList();

            viewModel.detail = new List<TBookingDetail>();
            int bdcount = 0;

            foreach (var group in processed.GroupBy(s => new { s.courtId, s.slotId }))
            {

                TimeOnly? currentStart = null;
                TimeOnly? currentEnd = null;

                foreach (var slot in group)
                {
                    TimeOnly slotBegin = TimeOnly.Parse(slot.Begin);
                    TimeOnly slotEnd = TimeOnly.Parse(slot.End);
                    if (currentStart == null)
                    {
                        currentStart = slotBegin;
                        currentEnd = slotEnd;
                    }
                    else if (slotBegin == currentEnd)
                    {
                        currentEnd = slotEnd;
                    }
                    else
                    {
                        viewModel.detail.Add(new TBookingDetail
                        {
                            BookingId = newBookingID,
                            DetailId = $"BD{_context.TBookingDetails.Count() + bdcount + 1:D3}",
                            CourtId = group.Key.courtId,
                            SlotId = group.Key.slotId,
                            StartTime = currentStart.Value,
                            EndTime = currentEnd.Value
                        });
                        bdcount++;
                        // bắt đầu chuỗi mới
                        currentStart = slotBegin;
                        currentEnd = slotEnd;
                    }
                }

                // lưu bản ghi cuối cùng trong nhóm
                if (currentStart != null)
                {
                    viewModel.detail.Add(new TBookingDetail
                    {
                        DetailId = $"BD{_context.TBookingDetails.Count() +bdcount + 1:D3}",
                        CourtId = group.Key.courtId,
                        SlotId = group.Key.slotId,
                        StartTime = currentStart.Value,
                        EndTime = currentEnd.Value
                    });
                    bdcount++;
                }
            }
            //tính tổng tiền và gán vào booking
            double totalPrice = 0;
            slots.ForEach(s =>
            {
                var price = _context.TPrices
                    .Where(p => p.CourtId == s.courtId && p.SlotId == s.slotId
                        && p.StartTime <= TimeOnly.Parse(s.startTime)
                        && p.EndTime >= TimeOnly.Parse(s.endTime))
                    .FirstOrDefault();
                if (price != null)
                {
                    totalPrice += price.UnitPrice;
                }
            });
            
            //cập nhật lại giá tiền
            viewModel.booking.Price = (float)totalPrice;

            //thông tin sân 
            viewModel.court = _context.TCourts
                .Where(c => c.CourtId == viewModel.detail.First().CourtId)
                .FirstOrDefault();
            viewModel.slot = _context.TSlots
                .Where(s => s.CourtId == viewModel.detail.First().CourtId)
                .ToList();

            ViewBag.TotalTime = slots.Count() / 2f;
            //viewmodel chứa thông tin booking(chưa có tổng tiền), booking detail, account, sân

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Confirm([FromBody] BookingInfoViewModel vm)
        {
            try
            {
                _context.TBookings.Add(vm.booking);
                _context.SaveChanges();

                foreach (var d in vm.detail)
                    d.BookingId = vm.booking.BookingId;

                _context.TBookingDetails.AddRange(vm.detail);
                _context.SaveChanges();

                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

    }
}
