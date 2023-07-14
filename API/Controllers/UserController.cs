using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private IUserManager UserManager { get; }
    private IConfiguration Configuration { get; }

    public UserController(IUserManager userManager, IConfiguration configuration)
    {
        UserManager = userManager;
        Configuration = configuration;
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var result = await UserManager.GetAllUsers();
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

    [HttpGet, Authorize(Roles = "Admin, Tập thể")]
    [Route("{depID}")]
    public async Task<IActionResult> GetUsersByDep(int depID)
    {
        try
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var claims = identity.FindAll(ClaimTypes.Role).ToArray();
                List<string> roles = new();
                foreach (var claim in claims)
                {
                    roles.Add(claim.Value);
                }
                List<UserDTO> result = new();
                if (roles[1] == "Admin")
                {
                    result = await UserManager.GetUsersByDep(depID);
                }
                else if (roles[1] == "User")
                {
                    result = await UserManager.GetUsersByDep(
                        Convert.ToInt32(identity.FindFirst("DepID")!.Value)
                    );
                }
                if (result.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(result);
            }
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInDTO request)
    {
        try
        {
            var result = await UserManager.LogIn(request);
            return ConfirmCredentials(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpDTO request)
    {
        try
        {
            var result = await UserManager.SignUp(request);
            return ConfirmCredentials(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    private IActionResult ConfirmCredentials(UserDTO result)
    {
        if (result.UserID > 0)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, Configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserID", result.UserID.ToString()),
                new Claim("DepID", result.DepID.ToString()),
                new Claim("role", result.CompRole.CompRoleName.ToString()),
                new Claim("role", result.SysRole.SysRoleName.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(180),
                signingCredentials: signIn
            );
            result.Jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(result);
        }
        return BadRequest("Please re-check username or password.");
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> EditUser(UserDTO request)
    {
        try
        {
            bool result = await UserManager.EditUser(request);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Please re-check input information.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
