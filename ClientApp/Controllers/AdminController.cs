using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class AdminController : Controller
{
    public IActionResult DepartmentManager()
    {
        return View();
    }

    public IActionResult InterfaceManager()
    {
        return View();
    }

    public IActionResult UserManager()
    {
        return View();
    }
}
