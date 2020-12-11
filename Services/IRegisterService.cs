using napper_be.Models;
using napper_be.Entities;

namespace napper_be.Services
{
    public interface IRegisterService
    {
        bool Register(User user);

    }
}