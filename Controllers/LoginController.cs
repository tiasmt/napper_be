using Microsoft.AspNetCore.Mvc;
using napper_be.Models;
using napper_be.Entities;
using napper_be.Services;

namespace napper_be.Controllers
{
    [ApiController]
    [Route("rest/api/latest/")]
    public class LoginController : ControllerBase
    {

        private readonly ISessionService _sessionService;
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        public LoginController(ILoginService loginService, IRegisterService registerService, ISessionService sessionService)
        {
             _loginService = loginService;
             _registerService = registerService; 
             _sessionService = sessionService;   
        }

        [HttpPost("login")]
        public IActionResult UserLogin([FromBody]User user)
        {
            var response = _loginService.Login(user.Username, user.Password);
            if(response == null)
                return BadRequest(new {message = "Username or password is incorrect."});
           
            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult UserLogout()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("User/{id}/{subcategory}")]
        public IActionResult GetUser(int id)
        {
            var user = _loginService.GetUserById(id);
            return Ok(user);
        }
    }
}

        