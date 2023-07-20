using ClientApp.Models;
using ClientApp.Models.Response;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace ClientApp.Controllers;

public class StatisticsController : Controller
{
    public IRequestHandler RequestHandler { get; set; }

    public StatisticsController(IRequestHandler requestHandler)
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
        var statisticsGet = await RequestHandler.GetAsync(
            "Meal/GetPersonalMonthlyStats/" + userID,
            HttpContext.Session.GetString("Jwt")!
        );

        if (mealGet.IsSuccessStatusCode && statisticsGet.IsSuccessStatusCode)
        {
            var mealList = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            if (mealList != null && mealList.Count > 0)
            {
                models.Meals = mealList;
                if (statisticsGet.ReasonPhrase != "No Content")
                {
                    var statList = await statisticsGet.Content.ReadFromJsonAsync<
                        List<PersonalMonthlyStatsModel>
                    >();
                    if (statList != null && statList.Count > 0)
                    {
                        models.Statistics = statList;
                        models.Totals = new List<int>();
                        foreach (var m in mealList)
                        {
                            models.Totals.Add(
                                statList
                                    .Select(
                                        s =>
                                            s.MealStats
                                                .Where(ms => ms.MealID == m.MealID)
                                                .Select(ms => ms.TotalServing)
                                                .FirstOrDefault()
                                    )
                                    .Sum()
                            );
                        }
                    }
                }
            }
            return View(models);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public IActionResult CompanyDaily()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        return View();
    }

    public IActionResult CompanyMonthly()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        dynamic models = new ExpandoObject();
        models.CurrentDate = DateTime.Now.ToString("yyyy-MM");
        models.Statistics = new List<CompanyMonthlyStatsModel>();
        models.Meals = new List<MealModel>();
        models.Departments = new List<DepartmentModel>();
        return View(models);
    }

    public async Task<IActionResult> FilterByMonth(IFormCollection collection)
    {
        dynamic models = new ExpandoObject();
        var mealGet = await RequestHandler.GetAsync(
            "Meal/GetAllMeals",
            HttpContext.Session.GetString("Jwt")!
        );
        var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");
        var statisticsGet = await RequestHandler.GetAsync(
            "Meal/GetCompanyMonthlyStats/" + collection["FilterMonth"],
            HttpContext.Session.GetString("Jwt")!
        );

        if (
            mealGet.IsSuccessStatusCode
            && depGet.IsSuccessStatusCode
            && statisticsGet.IsSuccessStatusCode
        )
        {
            models.Meals = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            models.Statistics = new List<CompanyMonthlyStatsModel>();
            if (statisticsGet.ReasonPhrase != "No Content")
            {
                models.Statistics = await statisticsGet.Content.ReadFromJsonAsync<
                    List<CompanyMonthlyStatsModel>
                >();
            }
            models.Departments = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
            models.CurrentDate = Convert.ToDateTime(collection["FilterMonth"]).ToString("yyyy-MM");
        }
        return View("CompanyMonthly", models);
    }
}
