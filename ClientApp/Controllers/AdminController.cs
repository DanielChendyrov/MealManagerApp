using ClientApp.Models.Binding;
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

    public async Task<IActionResult> DeleteDepartment(int depID)
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
                var response = await RequestHandler.DeleteAsync(
                    "Department/DeleteDep/" + depID,
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

    public async Task<IActionResult> UserManager(UserManagerModel model)
    {
        int? userID = HttpContext.Session.GetInt32("UserID");
        string? sysRole = HttpContext.Session.GetString("SysRole");
        if (userID == null || userID <= 0 || sysRole != "Admin")
        {
            return Redirect("/Home");
        }

        model.SortOrder ??= "Department";
        var userGet = await RequestHandler.GetAsync("User/GetAllUsers");
        var depGet = await RequestHandler.GetAsync("Department/GetAllDeps");
        var compRoleGet = await RequestHandler.GetAsync("Role/GetAllCompanyRoles");
        var sysRoleGet = await RequestHandler.GetAsync("Role/GetAllSystemRoles");

        if (
            userGet.IsSuccessStatusCode
            && depGet.IsSuccessStatusCode
            && compRoleGet.IsSuccessStatusCode
            && sysRoleGet.IsSuccessStatusCode
        )
        {
            var userList = await userGet.Content.ReadFromJsonAsync<List<UserModel>>();
            if (model.SortOrder == "Department")
            {
                model.Users = userList!.OrderBy(u => u.Dep!.DepName).ToList();
            }
            else if (model.SortOrder == "FullName")
            {
                model.Users = userList!.OrderBy(u => u.FullName).ToList();
            }
            var depList = await depGet.Content.ReadFromJsonAsync<List<DepartmentModel>>();
            model.Departments = depList!;
            var compRoleList = await compRoleGet.Content.ReadFromJsonAsync<List<CompRoleModel>>();
            model.CompRoles = compRoleList!;
            var sysRoleList = await sysRoleGet.Content.ReadFromJsonAsync<List<SysRoleModel>>();
            model.SysRoles = sysRoleList!;
        }
        return View(model);
    }

    public async Task<IActionResult> EditUsers(UserManagerModel model)
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
                    "User/EditUsers",
                    model.Users,
                    HttpContext.Session.GetString("Jwt")!
                );
            }
            return RedirectToAction("UserManager");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> DeleteUser(int uid)
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
                if (uid != userID)
                {
                    var response = await RequestHandler.DeleteAsync(
                        "User/DeleteUser/" + uid,
                        HttpContext.Session.GetString("Jwt")!
                    );
                }
            }
            return RedirectToAction("UserManager");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
