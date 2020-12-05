using System;
using napper_be.Models;
using napper_be.Repository;

namespace napper_be.Services
{
    public class SessionService
    {
        private const int DefaultSessionTimeout = 60;
        private readonly ISessionStorage _storage;
        public SessionService(ISessionStorage storage)
        {
            _storage = storage;
        }
        // public Session(int sessionTimeout, ISessionStorage storage)
        // {
        //     SessionTimeout = sessionTimeout;
        //     ExpirationDate = DateTime.Now.AddSeconds(sessionTimeout);
        //     _storage = storage;
        // }

        // public Session(int sessionId, string sessionGUID, int userId, int sessionTimeout, DateTime expirationDate, ISessionStorage storage)
        // {
        //     _sessionId = sessionId;
        //     _sessionGUID = sessionGUID;
        //     _sessionTimeout = sessionTimeout;
        //     _expirationDate = expirationDate;
        //     _userId = userId;
        //     _storage = storage;
        // }

        // public Session(ISessionStorage storage)
        // {
        //     SessionTimeout = DefaultSessionTimeout;
        //     ExpirationDate = DateTime.Now.AddSeconds(SessionTimeout);
        //     _storage = storage;
        // }

        public Session Open(int userId)
        {
            var session = new Session();
            session.UserId = userId;
            session.SessionGUID = Guid.NewGuid().ToString();
            session.ExpirationDate = DateTime.Now.AddSeconds(DefaultSessionTimeout);
            _storage.Create(session);
            return session;
        }

        public Session Update(string sessionId)
        {
            var session = Restore(sessionId);
            if (session.ExpirationDate < DateTime.Now)
                throw new Exception();

            session.ExpirationDate = DateTime.Now.AddSeconds(DefaultSessionTimeout);
            _storage.Update(session);
            return session;
        }

        public void Close(string sessionGUID)
        {
            var expirationDate = DateTime.Now;
            _storage.Delete(sessionGUID);
        }

        private Session Restore(string sessionId)
        {
            return _storage.GetBySessionId(sessionId);
        }
    }

}