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
            models.Servings = new List<ServingModel>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                models.Servings = (
                    await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>()
                )!.Where(s => s.UserID == uid);
            }
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> Department()
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
        var userGet = await RequestHandler.GetAsync(
            "User/GetUsersByDep/" + depID,
            Request.Cookies["jwt"]!
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
                models.Servings = (
                    await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>()
                )!.Where(s => s.UserID == uid);
            }
            models.Users = await userGet.Content.ReadFromJsonAsync<List<UserModel>>();
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> PersonalSubmit(IFormCollection collection)
    {
        try
        {
            int? uid = HttpContext.Session.GetInt32("UserID");
            if (uid == null || uid <= 0)
            {
                return Redirect("/Home");
            }

            if (collection["MealID"].Count > 0)
            {
                FormModel request =
                    new()
                    {
                        UserID = Convert.ToInt32(uid),
                        DepID = Convert.ToInt32(HttpContext.Session.GetInt32("DepID")),
                    };
                var mealID = collection["MealID"].ToList();
                var quantity = collection["Quantity"].ToList();
                var mealTime = collection["MealTime"].ToList();
                for (int i = 0; i < mealID.Count; i++)
                {
                    request.Servings.Add(
                        new ServingModel
                        {
                            Quantity = Convert.ToInt32(quantity[i]),
                            BookedDate = Convert
                                .ToDateTime(collection["BookedDate"])
                                .Add(TimeSpan.Parse(mealTime[i])),
                            MealID = Convert.ToInt32(mealID[i]),
                            UserID = Convert.ToInt32(uid),
                        }
                    );
                }

                var response = await RequestHandler.PostAsync(
                    "Meal/RegisterPersonalMeal",
                    request,
                    Request.Cookies["jwt"]!
                );
            }
            return RedirectToAction("Personal");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> DepartmentSubmit(IFormCollection collection)
    {
        try
        {
            int? uid = HttpContext.Session.GetInt32("UserID");
            if (uid == null || uid <= 0)
            {
                return Redirect("/Home");
            }
            return RedirectToAction("Department");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
