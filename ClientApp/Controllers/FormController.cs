using ClientApp.Models;
using ClientApp.Models.Request;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace ClientApp.Controllers;

public class FormController : Controller
{
    public IRequestHandler RequestHandler { get; set; }

    public FormController(IRequestHandler requestHandler)
    {
        RequestHandler = requestHandler;
    }

    public async Task<IActionResult> Personal()
    {
        int? uid = HttpContext.Session.GetInt32("UserID");
        if (uid == null || uid <= 0)
        {
            return Redirect("/Home");
        }

        dynamic models = new ExpandoObject();
        var mealGet = await RequestHandler.GetAsync("Meal/GetAllMeals", Request.Cookies["jwt"]!);
        int? depID = HttpContext.Session.GetInt32("DepID");
        var servingGet = await RequestHandler.GetAsync(
            "Meal/FindExistingRegistration/" + depID,
            Request.Cookies["jwt"]!
        );

        if (mealGet.IsSuccessStatusCode && servingGet.IsSuccessStatusCode)
        {
            models.Meals = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                models.Servings = (await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>())!.Where(s => s.UserID == uid);
            }
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public IActionResult Department()
    {
        return View();
    }

    public IActionResult PersonalSubmit()
    {
        return RedirectToAction("Personal");
    }
}
