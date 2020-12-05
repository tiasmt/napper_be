using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace napper_be.Controllers
{
    [ApiController]
    [Route("rest/api/latest/")]
    public class RegisterController : ControllerBase
    {

        private readonly Session _session;
        private readonly User _user;
        private LoginController _login;

        public RegisterController(IUserStorage userStorage, ISessionStorage sessionStorage)
        {
            _session = new Session(sessionStorage);
            _user = new User(userStorage);
            _login = new LoginController(sessionStorage, userStorage);
        }

        [HttpPost("register")]
        public IActionResult UserRegister(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _user.Register(user);
            _login.UserLogin(user);
            return Ok();
        }

        [HttpGet("user/{username}")]
        public string UserGetUser(string username)
        {
            _user.GetUser(username);
            return null;
        }
    }
}