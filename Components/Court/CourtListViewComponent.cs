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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courts = db.TCourts.ToList();
            return View("CourtList", courts);
        }
    }
}
