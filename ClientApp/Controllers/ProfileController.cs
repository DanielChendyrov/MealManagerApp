using ClientApp.Models.Binding;
using ClientApp.Models.Transfer;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class ProfileController : Controller
{
    public IRequestHandler RequestHandler { get; set; }

    public ProfileController(IRequestHandler requestHandler)
    {
        RequestHandler = requestHandler;
    }

    public async Task<IActionResult> ViewProfile()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        var userGet = await RequestHandler.GetAsync(
            "User/GetUserByID/" + userID,
            HttpContext.Session.GetString("Jwt")!
        );

        if (userGet.IsSuccessStatusCode)
        {
            var userModel = await userGet.Content.ReadFromJsonAsync<UserModel>();
            return View(userModel);
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> ChangePassword(IFormCollection collection)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null || userID <= 0)
            {
                return Redirect("/Home");
            }

            if (collection["NewPassword"].ToString() == collection["ConfirmPassword"].ToString())
            {
                ChangePasswordModel request =
                    new()
                    {
                        UserID = Convert.ToInt32(userID),
                        OldPassword = collection["OldPassword"].ToString(),
                        NewPassword = collection["NewPassword"].ToString(),
                    };
                var passwordChange = await RequestHandler.PutAsync(
                    "User/ChangePassword",
                    request,
                    HttpContext.Session.GetString("Jwt")!
                );

                if (passwordChange.IsSuccessStatusCode)
                {
                    ViewData["Error"] = "Đã đổi mật khẩu";
                }
                else
                {
                    ViewData["Error"] = passwordChange.StatusCode.ToString();
                }
                return View("ViewProfile");
            }
            ViewData["Error"] = "Mật khẩu mới không khớp.";
            return View("ViewProfile");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> ViewOrders()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID == null || userID <= 0)
        {
            return Redirect("/Home");
        }

        PersonalOrdersModel models = new();
        var servingGet = await RequestHandler.GetAsync(
            "Meal/GetAllPersonalOrders/" + userID,
            HttpContext.Session.GetString("Jwt")!
        );
        var mealGet = await RequestHandler.GetAsync(
            "Meal/GetAllMeals",
            HttpContext.Session.GetString("Jwt")!
        );

        if (servingGet.IsSuccessStatusCode && mealGet.IsSuccessStatusCode)
        {
            var mealList = await mealGet.Content.ReadFromJsonAsync<List<MealModel>>();
            models.Meals = mealList!;
            var orderList = new List<CustomOrder>();
            if (servingGet.ReasonPhrase != "No Content")
            {
                var servingList = await servingGet.Content.ReadFromJsonAsync<List<ServingModel>>();
                var dateList = servingList!.Select(s => s.BookedDate).Distinct().ToList();
                foreach (var date in dateList)
                {
                    orderList.Add(
                        new()
                        {
                            BookedDate = date,
                            Servings = servingList!
                                .Where(s => s.BookedDate == date)
                                .OrderBy(s => s.MealID)
                                .ToList(),
                        }
                    );
                }
            }
            models.Orders = orderList;
            return View(models);
        }
        return View();
    }

    public async Task<IActionResult> EditOrders(PersonalOrdersModel model)
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
                List<ServingModel> request = new();
                if (model.Orders.Count > 0)
                {
                    foreach (var o in model.Orders)
                    {
                        if (o.Servings.Count > 0)
                        {
                            foreach (var s in o.Servings)
                            {
                                request.Add(
                                    new()
                                    {
                                        ServingID = s.ServingID,
                                        Quantity = s.Quantity,
                                        BookedDate = o.BookedDate,
                                    }
                                );
                            }
                        }
                    }
                }
                var response = await RequestHandler.PutAsync(
                    "Meal/EditMeal",
                    request,
                    HttpContext.Session.GetString("Jwt")!
                );
            }
            return RedirectToAction("ViewOrders");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> DeleteOrder(string servingID)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID == null || userID <= 0)
            {
                return Redirect("/Home");
            }

            if (servingID != null)
            {
                var response = await RequestHandler.DeleteAsync(
                    "Meal/DeleteMeal/" + servingID,
                    HttpContext.Session.GetString("Jwt")!
                );
            }
            return RedirectToAction("ViewOrders");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> Manage3rdShift()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? compRole = HttpContext.Session.GetString("CompRole");
        if (userID == null || userID <= 0 || compRole != "Tập thể")
        {
            return Redirect("/Home");
        }

        return View();
    }
}
