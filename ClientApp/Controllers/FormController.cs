using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class FormController : Controller
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
