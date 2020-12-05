using System.Collections.Generic;
using napper_be.Models;

namespace napper_be.Repository
{
    public interface ISessionStorage
    {
        IEnumerable<Session> GetAll();

        Session GetBySessionId(string sessionId);

        void Create(Session session);

        void Update(Session session);

        void Delete(string sessionId);
    }
}