using napper_be.Entities;
using napper_be.Models;

namespace napper_be.Services
{
    public interface ILoginService
    {
        AuthenticateResponse Login(string username, string password);
        User GetUser(string username);
        User GetUserById(long userId);

    }
}