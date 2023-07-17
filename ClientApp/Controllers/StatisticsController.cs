using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Personal()
        {
            return View();
        }

        public IActionResult Department()
        {
            return View();
        }
    }
}
