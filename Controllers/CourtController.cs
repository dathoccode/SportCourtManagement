using Microsoft.AspNetCore.Mvc;

namespace SportCourtManagement.Controllers
{
    public class CourtController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Loading", "Account", new { target = Url.Action("CourtIndex", "Court") });
        }

        public IActionResult CourtIndex()
        {
            return View("Index");
        }

        // ====== [GET] TRANG BẢN ĐỒ ======
        public IActionResult Map()
        {
            return RedirectToAction("Loading", "Account", new { target = Url.Action("CourtMap", "Court") });
        }

        public IActionResult CourtMap()
        {
            return View("Map");
        }
    }
}
