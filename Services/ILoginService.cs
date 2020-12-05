using napper_be.Models;

namespace napper_be.Services
{
    public interface ILoginService
    {
        int Login(string username, string password);
        User GetUser(string username);

    }
}