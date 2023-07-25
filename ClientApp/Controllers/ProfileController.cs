using ClientApp.Models;
using ClientApp.Models.Request;
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

    public IActionResult ViewOrders()
    {
        return View();
    }
}
