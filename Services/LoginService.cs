using napper_be.Models;
using napper_be.Repository;
using napper_be.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using napper_be.Entities;
using napper_be.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;


namespace napper_be.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserStorage _storage;
        private readonly AppSettings _appSettings;
        public LoginService(IUserStorage storage, IOptions<AppSettings> appSettings)
        {
            _storage = storage;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Login(string username, string password)
        {
            var user = _storage.GetByUsername(username);
            var hashedPassword = PasswordUtils.HashPassword(user.Salt, password);
            if (hashedPassword == user.HashedPassword)
            {
                var token = generateJwtToken(user);
                return new AuthenticateResponse(user, token);
            }
            else
            {
                throw new NotFoundException($"Invalid username / password.");
            }
        }

        public User GetUser(string username)
        {
            return _storage.GetByUsername(username);
        }

        public User GetUserById(long userId)
        {
            return null;
        }

         private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }

}