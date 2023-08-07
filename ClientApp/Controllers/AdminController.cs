using ClientApp.Models.Transfer;
using ClientApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class AdminController : Controller
{
    public IRequestHandler RequestHandler { get; set; }

    public AdminController(IRequestHandler requestHandler)
    {
        RequestHandler = requestHandler;
    }

    public async Task<IActionResult> DepartmentManager()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? sysRole = HttpContext.Session.GetString("SysRole");
        if (userID == null || userID <= 0 || sysRole != "Admin")
        {
            return Redirect("/Home");
        }

        List<DepartmentModel> model = new();
        var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");

        if (depGet.IsSuccessStatusCode)
        {
            var depList = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
            model = depList!;
        }
        return View(model);
    }

    public async Task<IActionResult> EditDepartments(List<DepartmentModel> model)
    {
        try
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            string? sysRole = HttpContext.Session.GetString("SysRole");
            if (userID == null || userID <= 0 || sysRole != "Admin")
            {
                return Redirect("/Home");
            }

            if (ModelState.IsValid)
            {
                var response = await RequestHandler.PutAsync(
                    "Department/EditDep",
                    model,
                    HttpContext.Session.GetString("Jwt")!
                );
            }
            return RedirectToAction("DepartmentManager");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public IActionResult InterfaceManager()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? sysRole = HttpContext.Session.GetString("SysRole");
        if (userID == null || userID <= 0 || sysRole != "Admin")
        {
            return Redirect("/Home");
        }

        return View();
    }

    public async Task<IActionResult> UserManager()
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? sysRole = HttpContext.Session.GetString("SysRole");
        if (userID == null || userID <= 0 || sysRole != "Admin")
        {
            return Redirect("/Home");
        }

        List<UserModel> models = new();
        var userGet = await RequestHandler.GetAsync("User/GetAllUsers");

        if (userGet.IsSuccessStatusCode)
        {
            var userList = await userGet.Content.ReadFromJsonAsync<List<UserModel>>();
            models = userList!;
        }
        return View(models);
    }
}
