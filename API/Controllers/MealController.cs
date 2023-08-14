using BusinessObject.DTO;
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

    [HttpGet]
    [Route("{bookedDate}/{depID}")]
    public async Task<IActionResult> GetAll3rdShiftMeals(DateTime bookedDate, int depID)
    {
        try
        {
            var result = await MealManager.GetAll3rdShiftMeals(bookedDate, depID);
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

    [HttpGet]
    [Route("{uid}")]
    public async Task<IActionResult> GetAllPersonalOrders(int uid)
    {
        try
        {
            var result = await MealManager.GetAllPersonalOrders(uid);
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditMeal(List<ServingDTO> request)
    {
        try
        {
            var result = await MealManager.EditMeal(request);
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

    //Still needs improvements
    [HttpPut]
    public async Task<IActionResult> EditMeal3rdShift(List<ServingDTO> request)
    {
        try
        {
            var result = await MealManager.EditMeal3rdShift(request);
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

    [HttpDelete]
    [Route("{servingID}")]
    public async Task<IActionResult> DeleteMeal(int servingID)
    {
        try
        {
            var result = await MealManager.DeleteMeal(servingID);
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
