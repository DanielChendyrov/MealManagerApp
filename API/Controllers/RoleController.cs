using BusinessObject.DTO;
using BusinessObject.Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController, Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private IRoleManager RoleManager { get; }

    public RoleController(IRoleManager roleManager)
    {
        RoleManager = roleManager;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetAllCompanyRoles()
    {
        try
        {
            var result = await RoleManager.GetAllCompanyRoles();
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

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetAllSystemRoles()
    {
        try
        {
            var result = await RoleManager.GetAllSystemRoles();
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
    public async Task<IActionResult> AddRole<T>(T request)
    {
        try
        {
            var result = await RoleManager.AddRole(request);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditRole<T>(T request)
    {
        try
        {
            var result = await RoleManager.EditRole(request);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
