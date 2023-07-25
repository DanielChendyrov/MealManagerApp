using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class MealController : ControllerBase
{
    private IMealManager MealManager { get; }

    public MealController(IMealManager userManager)
    {
        MealManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMeals()
    {
        try
        {
            var result = await MealManager.GettAllMeals();
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

    [HttpGet]
    [Route("{uid}")]
    public async Task<IActionResult> GetPersonalMonthlyStats(int uid)
    {
        try
        {
            var result = await MealManager.GetPersonalMonthlyStats(uid);
            if (result.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize(Roles = "Admin")]
    [Route("{date}")]
    public async Task<IActionResult> GetCompanyDailyStats(string date)
    {
        try
        {
            var result = await MealManager.GetCompanyDailyStats(Convert.ToDateTime(date));
            if (result.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize(Roles = "Admin")]
    [Route("{date}")]
    public async Task<IActionResult> GetCompanyMonthlyStats(string date)
    {
        try
        {
            var result = await MealManager.GetCompanyMonthlyStats(Convert.ToDateTime(date));
            if (result.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("{depID}")]
    public async Task<IActionResult> FindExistingRegistration(int depID)
    {
        try
        {
            var result = await MealManager.FindExistingRegistration(depID);
            if (result.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPersonalMeal(FormDTO request)
    {
        try
        {
            var result = await MealManager.RegisterPersonalMeal(request);
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

    [HttpPost, Authorize(Roles = "Tập thể")]
    public async Task<IActionResult> RegisterDepartmentMeal(FormDTO request)
    {
        try
        {
            var result = await MealManager.RegisterDepartmentMeal(request);
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
