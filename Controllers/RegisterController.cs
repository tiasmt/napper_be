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
    public class RegisterController : ControllerBase
    {

        private readonly Session _session;
        private readonly User _user;

        public RegisterController(IUserStorage userStorage, ISessionStorage sessionStorage)
        {
            _session = new Session(sessionStorage);
            _user = new User(userStorage);
        }

        [HttpPost("register")]
        public string UserRegister([FromBody] User user)
        {
            _user.Register(user);
            return null;
        }

        [HttpGet("user/{username}")]
        public string UserGetUser(string username)
        {
            _user.GetUser(username);
            return null;
        }
    }
}