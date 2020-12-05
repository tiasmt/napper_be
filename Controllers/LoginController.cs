using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace napper_be.Controllers
{
    [ApiController]
    [Route("rest/api/latest/")]
    public class LoginController : ControllerBase
    {

        private readonly Session _session;
        private readonly User _user;
        public LoginController(ISessionStorage sessionStorage, IUserStorage userStorage)
        {
            _session = new Session(sessionStorage);
            _user = new User(userStorage);            
        }

        [HttpPost("login")]
        public IActionResult UserLogin([FromBody]User user)
        {
            var userId = _user.Login(user.Username, user.Password);
            if(userId > 0)
            {
                _session.Open(userId);
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

        