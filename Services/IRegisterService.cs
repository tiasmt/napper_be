using napper_be.Models;

namespace napper_be.Services
{
    public interface IRegisterService
    {
        bool Register(User user);

    }
}