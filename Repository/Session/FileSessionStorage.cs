using System;
using System.Collections.Generic;
using System.IO;
using napper_be.Models;

namespace napper_be.Repository
{
    public class FileSessionStorage : ISessionStorage
    {
        private const string SessionId = "SessionId:";
        private const string SessionGUID = "SessionGUID:";
        private const string UserId = "UserId:";
        private const string Version = "Version:";
        private const string SessionTimeout = "SessionTimeout:";
        private const string ExpirationDate = "ExpirationDate:";

        private readonly string _directoryPath;

        public FileSessionStorage(string directoryPath)
        {
            _directoryPath = directoryPath;
        }
        public void Create(Session session)
        {
            using (var file = new StreamWriter(File.Create(_directoryPath + session.SessionId + ".ses")))
            {
                file.WriteLine($"{SessionId}{session.SessionId}");
                file.WriteLine($"{UserId}{session.UserId}");
                file.WriteLine($"{SessionTimeout}{session.SessionTimeout}");
                file.WriteLine($"{ExpirationDate}{session.ExpirationDate}");
            }
        }

        public void Delete(string sessionId)
        {
            File.Delete(_directoryPath + sessionId + ".ses");
        }

        public IEnumerable<Session> GetAll()
        {
            var sessions = new List<Session>();
            foreach (var file in Directory.EnumerateFiles(_directoryPath))
            {
                var fileStream = new FileStream(file, FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    var session = new Session();
                    session.SessionId = int.Parse(reader.ReadLine().Substring(SessionId.Length));
                    session.SessionGUID = reader.ReadLine().Substring(SessionGUID.Length);
                    session.UserId = int.Parse(reader.ReadLine().Substring(UserId.Length));
                    session.SessionTimeout = int.Parse(reader.ReadLine().Substring(SessionTimeout.Length));
                    session.ExpirationDate = DateTime.Parse(reader.ReadLine().Substring(ExpirationDate.Length));
                    sessions.Add(session);
                }
            }
            return sessions;
        }

        public Session GetBySessionId(string sessionId)
        {
            Session session = new Session();

            var fileStream = new FileStream(_directoryPath + sessionId + ".ses", FileMode.Open);
            using (var reader = new StreamReader(fileStream))
            {
                session.SessionId = int.Parse(reader.ReadLine().Substring(SessionId.Length));
                session.SessionGUID = reader.ReadLine().Substring(SessionGUID.Length);
                session.UserId = int.Parse(reader.ReadLine().Substring(UserId.Length));
                session.SessionTimeout = int.Parse(reader.ReadLine().Substring(SessionTimeout.Length));
                session.ExpirationDate = DateTime.Parse(reader.ReadLine().Substring(ExpirationDate.Length));
            }

            return session;
        }

        public void Update(Session session)
        {
            using (var file = new StreamWriter(File.OpenWrite(_directoryPath + session.SessionId + ".ses")))
            {
                file.WriteLine($"SessionId:{session.SessionId}");
                file.WriteLine($"UserId:{session.UserId}");
                file.WriteLine($"SessionTimeout:{session.SessionTimeout}");
                file.WriteLine($"ExpirationDate:{session.ExpirationDate}");
            }
        }
    }

}