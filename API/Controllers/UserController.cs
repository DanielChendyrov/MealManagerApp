using BusinessObject.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserManager UserManager { get; }

        public UserController(IUserManager userManager)
        {
            UserManager = userManager;
        }
    }
}
