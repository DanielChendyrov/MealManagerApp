using BusinessObject.DTO;
using BusinessObject.Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController, Authorize(Roles = "Admin")]
public class DepartmentController : ControllerBase
{
    private IDepartmentManager DepartmentManager { get; }

    public DepartmentController(IDepartmentManager departmentManager)
    {
        DepartmentManager = departmentManager;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetAllDeps()
    {
        try
        {
            var result = await DepartmentManager.GetAllDeps();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewDep(List<DepartmentDTO> request)
    {
        try
        {
            var result = await DepartmentManager.CreateNewDep(request);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditDep(List<DepartmentDTO> depList)
    {
        try
        {
            var result = await DepartmentManager.EditDep(depList);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
