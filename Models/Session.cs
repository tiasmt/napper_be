using System;

namespace napper_be
{
    public class Session
    {
        private const int DefaultSessionTimeout = 60;
        private readonly ISessionStorage _storage;
        private int _sessionId;
        private string _sessionGUID;
        private int _sessionTimeout;
        private DateTime _expirationDate;
        private int _userId;

        public int SessionId { get => _sessionId; set => _sessionId = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public int SessionTimeout { get => _sessionTimeout; set => _sessionTimeout = value; }
        public DateTime ExpirationDate { get => _expirationDate; set => _expirationDate = value; }
        public string SessionGUID { get => _sessionGUID; set => _sessionGUID = value; }
        public Session()
        {

        }
        public Session(int sessionTimeout, ISessionStorage storage)
        {
            SessionTimeout = sessionTimeout;
            ExpirationDate = DateTime.Now.AddSeconds(sessionTimeout);
            _storage = storage;
        }

        public Session(int sessionId, string sessionGUID, int userId, int sessionTimeout, DateTime expirationDate, ISessionStorage storage)
        {
            _sessionId = sessionId;
            _sessionGUID = sessionGUID;
            _sessionTimeout = sessionTimeout;
            _expirationDate = expirationDate;
            _userId = userId;
            _storage = storage;
        }

        public Session(ISessionStorage storage)
        {
            SessionTimeout = DefaultSessionTimeout;
            ExpirationDate = DateTime.Now.AddSeconds(SessionTimeout);
            _storage = storage;
        }

        public Session Open(int userId)
        {
            _userId = userId;
            _sessionGUID = Guid.NewGuid().ToString();
            ExpirationDate = DateTime.Now.AddSeconds(SessionTimeout);
            _storage.Create(this);
            return this;
        }

        public Session Update(string sessionId)
        {
            Restore(sessionId);
            if (ExpirationDate < DateTime.Now)
                throw new Exception();

            ExpirationDate = DateTime.Now.AddSeconds(SessionTimeout);
            _storage.Update(this);
            return this;
        }

        public Session Close()
        {
            ExpirationDate = DateTime.Now;
            _storage.Delete(_sessionGUID);

            return this;
        }

        private void Restore(string sessionId)
        {
            var session = _storage.GetBySessionId(sessionId);
            _sessionId = session._sessionId;
            SessionTimeout = session.SessionTimeout;
            ExpirationDate = session.ExpirationDate;
        }
    }
}

