using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(
            IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> UserLogin(UserFormDto userForm)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var token = await userRepository.Login(userForm.Email, userForm.Password, ipAddress);
            if(token != null)
            {
                return Ok(new { bearerToken = token });
            }
            return NotFound();
        }

        [HttpPost("register")]
        public async Task<ActionResult> UserRegister(UserFormDto userForm)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            if(await userRepository.Register(userForm, ipAddress))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
