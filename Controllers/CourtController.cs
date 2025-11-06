using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SportCourtManagement.Services.API;
using SportCourtManagement.Services.Data;
using SportCourtManagement.Models;


namespace SportCourtManagement.Controllers
{
    public class CourtController : Controller
    {
        private readonly GeoCodingService geoCoding;
        private readonly QuanLySanTheThaoContext db;

        public CourtController(GeoCodingService geocodingService, QuanLySanTheThaoContext _context)
        {
            geoCoding = geocodingService;
            db = _context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Loading", "Account", new { target = Url.Action("CourtIndex", "Court") });
        }

        public IActionResult Map()
        {      
            
            return View();
        }

        public IActionResult FilterByKeyword(string keyword, string id)
        {
            return ViewComponent("CourtList", new { keyword, id});
        }

        [HttpGet]
        public async Task<IActionResult> GetCourts()
        {
            try
            {
                var courts = db.TCourts.ToList();

                int updatedCount = 0;
                foreach (var c in courts)
                {
                    if (c.Latitude == null || c.Longitude == null)
                    {
                        var res = await geoCoding.GetCoordinatesAsync(c.CourtAddress);
                        if (res == null) continue;

                        c.Latitude = res.Value.lat;
                        c.Longitude = res.Value.lon;
                        updatedCount++;

                        if (updatedCount % 5 == 0) // cứ 5 sân lưu 1 lần
                            await db.SaveChangesAsync();
                    }
                }
                if (updatedCount % 5 != 0)
                    await db.SaveChangesAsync();


                var result = courts
                    .Where(c => c.Latitude.HasValue && c.Longitude.HasValue)
                    .Select(c => new {
                        c.CourtId,
                        c.SportId,
                        c.CourtName,
                        c.CourtAddress,
                        c.Latitude,
                        c.Longitude
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log this in console output for debugging
                Console.WriteLine("ERROR in GetCourts: " + ex.Message);
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpGet("Court/GetSlotsDetails/{courtId}")]
        public async Task<IActionResult> GetSlotsDetails(string courtId, DateTime? date)
        {
            var result = db.TCourts
                .Where(c => c.CourtId == courtId)
                .Select(c => new
                {
                    c.CourtId,
                    c.CourtName,
                    c.OpenTime,
                    c.CloseTime,
                    Slots = db.TSlots
                        .Where(s => s.CourtId == c.CourtId)
                        .Select(s => new
                        {
                            s.SlotId,
                            s.SlotName,
                            Bookings = db.TBookingDetails
                                .Where(bd => bd.SlotId == s.SlotId && bd.CourtId == courtId)
                                .Join(db.TBookings,
                                    bd => bd.BookingId,
                                    b => b.BookingId,
                                    (bd, b) => new
                                    {
                                        b.BookingId,
                                        b.BookingDate,
                                        bd.StartTime,
                                        bd.EndTime
                                    }).Where(b => !date.HasValue || b.BookingDate.Date == date.Value.Date)
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefault();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Booking(string courtId)
        {
            if (courtId != null)
            {
                var court = db.TCourts.FirstOrDefault(c => c.CourtId == courtId);
                if (court == null)
                    return NotFound();
                ViewBag.CourtID = courtId;
                return View(court);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSportTypes()
        {
            var res = db.TSports.ToList();
            return Ok(res);
        }

    }
}
