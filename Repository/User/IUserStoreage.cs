using System.Collections.Generic;

namespace napper_be
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