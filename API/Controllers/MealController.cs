using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class MealController : ControllerBase
{

}
