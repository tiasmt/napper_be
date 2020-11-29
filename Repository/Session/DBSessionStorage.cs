using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace napper_be
{
    public class DBSessionStorage : ISessionStorage
    {
        private readonly string _connectionString;
        public DBSessionStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DevConnection");
        }
        public void Create(Session session)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"INSERT INTO sessions (user_id, session_guid, timeout, expirate_date) 
                                                VALUES (@user_id, @session_guid, @timeout, @expirate_date)", connection))
                {
                    cmd.Parameters.AddWithValue("user_id", session.UserId);
                    cmd.Parameters.AddWithValue("session_guid", session.SessionGUID);
                    cmd.Parameters.AddWithValue("timeout", session.SessionTimeout);
                    cmd.Parameters.AddWithValue("expirate_date", session.ExpirationDate);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public void Delete(string sessionGUID)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"DELETE FROM sessions WHERE session_guid = @sessionGUID", connection))
                {
                    cmd.Parameters.AddWithValue("sessionGUID", sessionGUID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public IEnumerable<Session> GetAll()
        {
            var sessions = new List<Session>();
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"SELECT * FROM sessions", connection))
                {
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var session = new Session();
                            session.SessionId = dr.GetFieldValue<int>("id");
                            session.SessionGUID = dr.GetFieldValue<string>("session_guid");
                            session.SessionTimeout = dr.GetFieldValue<int>("session_timeout");
                            session.ExpirationDate = dr.GetFieldValue<DateTime>("expirate_date");
                            sessions.Add(session);
                        }
                    }
                }
                return sessions;
            }
            catch (Exception ex)
            {
                throw;
                //log exception
            }
        }

        public Session GetBySessionId(string sessionGUID)
        {
            var session = new Session();
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"SELECT * FROM sessions WHERE session_guid = @sessionGUID", connection))
                {
                    cmd.Parameters.AddWithValue("sessionGUID", sessionGUID);
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            session.SessionId = dr.GetFieldValue<int>("id");
                            session.SessionGUID = dr.GetFieldValue<string>("session_guid");
                            session.SessionTimeout = dr.GetFieldValue<int>("session_timeout");
                            session.ExpirationDate = dr.GetFieldValue<DateTime>("expirate_date");
                        }
                    }
                }
                return session;
            }
            catch (Exception ex)
            {
                throw;
                //log exception
            }
        }

        public void Update(Session session)
        {
            throw new NotImplementedException();
        }
    }

}