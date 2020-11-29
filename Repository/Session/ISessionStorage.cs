using System.Collections.Generic;

namespace napper_be
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