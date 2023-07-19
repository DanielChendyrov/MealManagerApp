using ClientApp.Models;
using ClientApp.Models.Request;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;

namespace ClientApp.Controllers;

public class AuthenticateController : Controller
{
    public IRequestHandler RequestHandler { get; set; }

    public AuthenticateController(IRequestHandler requestHandler)
    {
        RequestHandler = requestHandler;
    }

    public IActionResult LogIn()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID != null && userID > 0)
        {
            return Redirect("/Home");
        }
        return View();
    }

    public async Task<IActionResult> SignUp()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        if (userID != null && userID > 0)
        {
            return Redirect("/Home");
        }

        dynamic models = new ExpandoObject();
        var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");
        var compRoleGet = await RequestHandler.GetAsync("Role/GetAllCompanyRoles");
        var sysRoleGet = await RequestHandler.GetAsync("Role/GetAllSystemRoles");
        if (
            depGet.IsSuccessStatusCode
            && compRoleGet.IsSuccessStatusCode
            && sysRoleGet.IsSuccessStatusCode
        )
        {
            models.Departments = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
            models.CompRoles = await compRoleGet.Content.ReadFromJsonAsync<List<CompRoleModel>>();
            models.SysRoles = await sysRoleGet.Content.ReadFromJsonAsync<List<SysRoleModel>>();
            if (models.Departments != null && models.CompRoles != null && models.SysRoles != null)
            {
                return View(models);
            }
        }
        ViewData["Error"] =
            "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
        return View();
    }

    public async Task<IActionResult> LogInRequest(LogInModel request)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID != null && userID > 0)
            {
                return Redirect("/Home");
            }
            return await ConfirmCredentials(request, "LogIn");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> SignUpRequest(IFormCollection collection)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID != null && userID > 0)
            {
                return Redirect("/Home");
            }
            SignUpModel request =
                new()
                {
                    FullName = collection["FullName"].ToString(),
                    Username = collection["Username"].ToString(),
                    Password = collection["Password"].ToString(),
                    DepID = Convert.ToInt32(collection["DepID"]),
                    CompRoleID = Convert.ToInt32(collection["CompRoleID"]),
                    SysRoleID = Convert.ToInt32(collection["SysRoleID"]),
                };
            return await ConfirmCredentials(request, "SignUp");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<IActionResult> ConfirmCredentials<T>(T request, string path)
    {
        if (Validate(request))
        {
            var response = await RequestHandler.PostAsync("User/" + path, request);
            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<UserModel>(
                    await response.Content.ReadAsStringAsync()
                );
                if (content != null)
                {
                    HttpContext.Session.SetInt32("UserID", content.UserID);
                    HttpContext.Session.SetString("FullName", content.FullName!);
                    HttpContext.Session.SetInt32("DepID", content.Dep!.DepID);
                    HttpContext.Session.SetString("DepName", content.Dep.DepName!);
                    HttpContext.Session.SetString("CompRole", content.CompRole!.CompRoleName!);
                    HttpContext.Session.SetString("SysRole", content.SysRole!.SysRoleName!);
                    HttpContext.Session.SetString("Jwt", content.Jwt!);
                    return Redirect("/Home");
                }
                return RedirectToAction(path);
            }
            return RedirectToAction(path);
        }
        return RedirectToAction(path);
    }

    private static bool Validate<T>(T request)
    {
        if (request != null)
        {
            foreach (var prop in request.GetType().GetProperties())
            {
                var value = prop.GetValue(request);
                if (value == null)
                {
                    return false;
                }
                if (value.GetType() == typeof(string) && (string)value == "")
                {
                    return false;
                }
                if (value.GetType() == typeof(int) && (int)value == 0)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
