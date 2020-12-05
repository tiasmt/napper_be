using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using napper_be.Models;
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
            var userId = _loginService.Login(user.Username, user.Password);
            if(userId > 0)
            {
                _sessionService.Open(userId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("logout")]
        public IActionResult UserLogout()
        {
            return Ok();
        }
    }
}

        