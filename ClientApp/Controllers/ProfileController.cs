using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult ViewProfile()
        {
            return View();
        }

        public IActionResult ViewOrders()
        {
            return View();
        }
    }
}
