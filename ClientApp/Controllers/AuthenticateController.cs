﻿using ClientApp.Models;
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
        int? uid = HttpContext.Session.GetInt32("UserID");
        if (uid != null && uid > 0)
        {
            return Redirect("/Home");
        }
        return View();
    }

    public async Task<IActionResult> SignUp()
    {
        int? uid = HttpContext.Session.GetInt32("UserID");
        if (uid == null || uid <= 0)
        {
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
                var depModel = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
                var compRoleModel = await compRoleGet.Content.ReadFromJsonAsync<
                    List<CompRoleModel>
                >();
                var sysRoleModel = await sysRoleGet.Content.ReadFromJsonAsync<List<SysRoleModel>>();

                if (depModel != null && compRoleModel != null && sysRoleModel != null)
                {
                    models.Departments = depModel;
                    models.CompRoles = compRoleModel;
                    models.SysRoles = sysRoleModel;
                    return View(models);
                }
            }
            ViewData["Error"] =
                "Đang có lỗi xảy ra với kết nối tới máy chủ, xin hãy làm mới trang web hoặc quay lại lúc khác.";
            return View();
        }
        return Redirect("/Home");
    }

    public async Task<IActionResult> LogInRequest(LogInModel request)
    {
        try
        {
            int? uid = HttpContext.Session.GetInt32("UserID");
            if (uid != null && uid > 0)
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
            int? uid = HttpContext.Session.GetInt32("UserID");
            if (uid != null && uid > 0)
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
                    HttpContext.Session.SetString("FullName", content.FullName);
                    HttpContext.Session.SetInt32("DepID", content.Dep.DepID);
                    HttpContext.Session.SetString("DepName", content.Dep.DepName);
                    HttpContext.Session.SetString("CompRole", content.CompRole.CompRoleName);
                    HttpContext.Session.SetString("SysRole", content.SysRole.SysRoleName);
                    Response.Cookies.Append(
                        "Jwt",
                        content.Jwt,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(30),
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        }
                    );
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
