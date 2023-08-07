using ClientApp.Models.Binding;
using ClientApp.Models.Transfer;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;

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

        PersonalStatsModel model = new();
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
            model.Meals = mealList!;
            if (statisticsGet.ReasonPhrase != "No Content")
            {
                var statList = await statisticsGet.Content.ReadFromJsonAsync<
                    List<PersonalMonthlyStats>
                >();
                if (mealList != null && mealList.Count > 0)
                {
                    model.Statistics = statList!;
                    model.Totals = new List<int>();
                    foreach (var m in mealList)
                    {
                        model.Totals.Add(
                            statList!
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
            return View(model);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> CompanyMonthly(CompanyMonthlyStatsModel model)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? sysRole = HttpContext.Session.GetString("SysRole");
            if (userID == null || userID <= 0 || sysRole != "Admin")
            {
                return Redirect("/Home");
            }

            model.ChosenDate ??= DateTime.Now.ToString("yyyy-MM");
            var mealGet = await RequestHandler.GetAsync(
                "Meal/GetAllMeals",
                HttpContext.Session.GetString("Jwt")!
            );
            var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");
            var statisticsGet = await RequestHandler.GetAsync(
                "Meal/GetCompanyMonthlyStats/" + model.ChosenDate,
                HttpContext.Session.GetString("Jwt")!
            );

            if (
                mealGet.IsSuccessStatusCode
                && depGet.IsSuccessStatusCode
                && statisticsGet.IsSuccessStatusCode
            )
            {
                var mealList = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
                model.Meals = mealList!;
                var depList = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
                model.Departments = depList!;
                if (statisticsGet.ReasonPhrase != "No Content")
                {
                    var statList = await statisticsGet.Content.ReadFromJsonAsync<
                        List<CompanyMonthlyStats>
                    >();
                    model.Statistics = statList!;
                }
            }
            return View(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> CompanyDaily(CompanyDailyStatsModel model)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? sysRole = HttpContext.Session.GetString("SysRole");
            if (userID == null || userID <= 0 || sysRole != "Admin")
            {
                return Redirect("/Home");
            }

            model.ChosenDate ??= DateTime.Now.ToString("yyyy-MM-dd");
            var mealGet = await RequestHandler.GetAsync(
                "Meal/GetAllMeals",
                HttpContext.Session.GetString("Jwt")!
            );
            var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");
            var statisticsGet = await RequestHandler.GetAsync(
                "Meal/GetCompanyDailyStats/" + model.ChosenDate,
                HttpContext.Session.GetString("Jwt")!
            );

            if (
                mealGet.IsSuccessStatusCode
                && depGet.IsSuccessStatusCode
                && statisticsGet.IsSuccessStatusCode
            )
            {
                var mealList = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
                model.Meals = mealList!;
                var depList = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
                model.Departments = depList!;
                if (statisticsGet.ReasonPhrase != "No Content")
                {
                    var statList = await statisticsGet.Content.ReadFromJsonAsync<
                        List<ServingModel>
                    >();
                    if (mealList != null)
                    {
                        var statModel = new List<CompanyDailyStats>();
                        foreach (var m in mealList)
                        {
                            statModel.Add(
                                new CompanyDailyStats()
                                {
                                    MealID = m.MealID,
                                    MealName = m.MealName,
                                    Servings = statList!.Where(s => s.MealID == m.MealID).ToList(),
                                    Total = statList!
                                        .Where(s => s.MealID == m.MealID)
                                        .Select(s => s.Quantity)
                                        .Sum(),
                                }
                            );
                        }
                        model.Statistics = statModel;
                    }
                }
            }
            return View(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
