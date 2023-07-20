using ClientApp.Models;
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

    public IActionResult ViewOrders()
    {
        return View();
    }
}
