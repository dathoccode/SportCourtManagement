using Microsoft.AspNetCore.Mvc;

namespace SportCourtManagement.Controllers
{
    public class CourtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Map()
        {

            return View();
        }
    }
}
