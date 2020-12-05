using napper_be.Models;
using napper_be.Repository;

namespace napper_be.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserStorage _storage;
        public LoginService(IUserStorage storage)
        {
            _storage = storage;
        }

        public int Login(string username, string password)
        {
            var user = _storage.GetByUsername(username);
            var hashedPassword = PasswordUtils.HashPassword(user.Salt, password);
            if (hashedPassword == user.HashedPassword)
            {
                return user.Id;
            }
            else
            {
                return 0;
            }
        }

        public User GetUser(string username)
        {
            return _storage.GetByUsername(username);
        }


    }

}