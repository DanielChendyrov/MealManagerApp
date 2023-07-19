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
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        dynamic models = new ExpandoObject();
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
            models.Meals = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            models.Servings = new List<ServingModel>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                models.Servings = (
                    await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>()
                )!
                    .Where(s => s.UserID == userID)
                    .ToList();
            }
            models.ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> Department()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        dynamic models = new ExpandoObject();
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
                models.Servings = (
                    await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>()
                )!
                    .Where(s => s.UserID == userID)
                    .ToList();
            }
            models.Users = await userGet.Content.ReadFromJsonAsync<List<UserModel>>();
            models.ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null || userID <= 0)
            {
                return Redirect("/Home");
            }

            if (collection["MealID"].Count > 0)
            {
                FormModel request =
                    new()
                    {
                        UserID = Convert.ToInt32(userID),
                        DepID = Convert.ToInt32(HttpContext.Session.GetInt32("DepID")),
                    };
                var mealIDCol = collection["MealID"].ToList();
                var quantityCol = collection["Quantity"].ToList();
                var mealTimeCol = collection["MealTime"].ToList();
                for (int i = 0; i < mealIDCol.Count; i++)
                {
                    request.Servings.Add(
                        new ServingModel
                        {
                            Quantity = Convert.ToInt32(quantityCol[i]),
                            BookedDate = Convert
                                .ToDateTime(collection["BookedDate"])
                                .Add(TimeSpan.Parse(mealTimeCol[i])),
                            MealID = Convert.ToInt32(mealIDCol[i]),
                            UserID = Convert.ToInt32(userID),
                        }
                    );
                }

                var response = await RequestHandler.PostAsync(
                    "Meal/RegisterPersonalMeal",
                    request,
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

    public async Task<IActionResult> DepartmentSubmit(IFormCollection collection)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null || userID <= 0)
            {
                return Redirect("/Home");
            }

            if (collection["UserID"].Count > 0)
            {
                FormModel request =
                    new()
                    {
                        UserID = Convert.ToInt32(userID),
                        DepID = Convert.ToInt32(HttpContext.Session.GetInt32("DepID")),
                    };
                var userIDCol = collection["UserID"].ToList();
                foreach (var u in userIDCol)
                {
                    if (collection["MealID" + u].Count > 0)
                    {
                        var mealIDCol = collection["MealID" + u].ToList();
                        var quantityCol = collection["Quantity" + u].ToList();
                        var mealTimeCol = collection["MealTime" + u].ToList();
                        for (int i = 0; i < mealIDCol.Count; i++)
                        {
                            request.Servings.Add(
                                new ServingModel
                                {
                                    Quantity = Convert.ToInt32(quantityCol[i]),
                                    BookedDate = Convert
                                        .ToDateTime(collection["BookedDate"])
                                        .Add(TimeSpan.Parse(mealTimeCol[i])),
                                    MealID = Convert.ToInt32(mealIDCol[i]),
                                    UserID = Convert.ToInt32(u),
                                }
                            );
                        }
                    }
                }

                if (request.Servings.Count > 0)
                {
                    var response = await RequestHandler.PostAsync(
                        "Meal/RegisterPersonalMeal",
                        request,
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
