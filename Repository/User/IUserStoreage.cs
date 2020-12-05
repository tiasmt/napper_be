using System.Collections.Generic;
using napper_be.Models;

namespace napper_be.Repository
{
    public interface IUserStorage
    {
        IEnumerable<User> GetAll();

        User GetByUsername(string username);

        void Create(User user);

        void Update(User user);

        void Delete(string userId);
    }
}