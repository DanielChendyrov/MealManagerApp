using ClientApp.Models.Binding;
using ClientApp.Models.Transfer;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;

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
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        FormModel model = new();
        var mealGet = await RequestHandler.GetAsync(
            "Meal/GetAllMeals",
            HttpContext.Session.GetString("Jwt")!
        );
        int? depID = HttpContext.Session.GetInt32("DepID");
        var servingGet = await RequestHandler.GetAsync(
            "Meal/FindExistingRegistration/" + depID,
            HttpContext.Session.GetString("Jwt")!
        );

        if (mealGet.IsSuccessStatusCode && servingGet.IsSuccessStatusCode)
        {
            model.Meals = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            model.Servings = new List<ServingModel>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                model.Servings = (await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>())!
                    .Where(s => s.UserID == userID)
                    .ToList();
            }
            model.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(model);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> Department()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? compRole = HttpContext.Session.GetString("CompRole");
        if (userID == null || userID <= 0 || compRole != "Tập thể")
        {
            return Redirect("/Home");
        }

        FormModel models = new();
        var mealGet = await RequestHandler.GetAsync(
            "Meal/GetAllMeals",
            HttpContext.Session.GetString("Jwt")!
        );
        int? depID = HttpContext.Session.GetInt32("DepID");
        var servingGet = await RequestHandler.GetAsync(
            "Meal/FindExistingRegistration/" + depID,
            HttpContext.Session.GetString("Jwt")!
        );
        var userGet = await RequestHandler.GetAsync(
            "User/GetUsersByDep/" + depID,
            HttpContext.Session.GetString("Jwt")!
        );

        if (
            mealGet.IsSuccessStatusCode
            && servingGet.IsSuccessStatusCode
            && userGet.IsSuccessStatusCode
        )
        {
            models.Meals = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            models.Servings = new List<ServingModel>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                models.Servings = await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>();
            }
            models.Users = await userGet.Content.ReadFromJsonAsync<List<UserModel>>();
            models.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> PersonalSubmit(FormModel model)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null || userID <= 0)
            {
                return Redirect("/Home");
            }

            if (ModelState.IsValid)
            {
                foreach (var s in model.Servings)
                {
                    s.UserID = model.UserID;
                    s.BookedDate = Convert.ToDateTime(model.BookedDate).Add(s.Meal!.Time);
                }
                var response = await RequestHandler.PostAsync(
                    "Meal/RegisterPersonalMeal",
                    model,
                    HttpContext.Session.GetString("Jwt")!
                );
            }
            return RedirectToAction("Personal");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> DepartmentSubmit(FormModel model)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? compRole = HttpContext.Session.GetString("CompRole");
            if (userID == null || userID <= 0 || compRole != "Tập thể")
            {
                return Redirect("/Home");
            }

            if (ModelState.IsValid)
            {
                foreach(var s in model.Servings)
                {
                    s.BookedDate = Convert.ToDateTime(model.BookedDate).Add(s.Meal!.Time);
                }

                if (model.Servings.Count > 0)
                {
                    var response = await RequestHandler.PostAsync(
                        "Meal/RegisterDepartmentMeal",
                        model,
                        HttpContext.Session.GetString("Jwt")!
                    );
                }
            }
            return RedirectToAction("Department");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
