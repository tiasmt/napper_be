using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using napper_be.Services;
using napper_be.Models;

namespace napper_be.Controllers
{
    [ApiController]
    [Route("rest/api/latest/")]
    public class RegisterController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;


        public RegisterController(IRegisterService registerService, ILoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }

        [HttpPost("register")]
        public IActionResult UserRegister(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _registerService.Register(user);
            _loginService.Login(user.Username, user.Password);
            return Ok();
        }

        [HttpGet("user/{username}")]
        public string UserGetUser(string username)
        {
            _loginService.GetUser(username);
            return null;
        }
    }
}