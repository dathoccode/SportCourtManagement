using Microsoft.AspNetCore.Mvc;
using SportCourtManagement.Services.Data;

namespace SportCourtManagement.Components.Court
{
    public class CourtTypesViewComponent: ViewComponent
    {
        private readonly QuanLySanTheThaoContext db;

        public CourtTypesViewComponent(QuanLySanTheThaoContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sports = db.TSports.ToList();
            return View("CourtTypes", sports);
        }
    }
}
