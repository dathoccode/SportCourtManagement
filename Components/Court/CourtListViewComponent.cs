using Microsoft.AspNetCore.Mvc;
using SportCourtManagement.Services.Data;

namespace SportCourtManagement.Components.Court
{
    public class CourtListViewComponent : ViewComponent
    {
        private readonly QuanLySanTheThaoContext db;

        public CourtListViewComponent(QuanLySanTheThaoContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? keyword, string? id)
        {
           var courts = db.TCourts
                .Where(c => string.IsNullOrEmpty(keyword) || c.CourtName.ToLower().Contains(keyword))
                .Where(c => string.IsNullOrEmpty(id) || c.SportId == id)
                .ToList();
            return View("CourtList", courts);
        }
    }
}
