using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using napper_be.Repository;
using napper_be.Models;

namespace napper_be
{
    public class DBUserStorage : IUserStorage
    {
        private readonly string _connectionString;
        public DBUserStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DevConnection");

        }

        public void Create(User user)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"INSERT INTO users (username, email, name, surname, password, salt) 
                                                VALUES (@username, @email, @name, @surname, @password, @salt)", connection))
                {
                    cmd.Parameters.AddWithValue("username", user.Username);
                    cmd.Parameters.AddWithValue("email", user.Email);
                    cmd.Parameters.AddWithValue("name", user.Name);
                    cmd.Parameters.AddWithValue("surname", user.Surname);
                    cmd.Parameters.AddWithValue("password", user.HashedPassword);
                    cmd.Parameters.AddWithValue("salt", Convert.ToBase64String(user.Salt));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //log exception
            }

        }

        public void Delete(string username)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"DELETE FROM users WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //log exception
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"SELECT * FROM users", connection))
                {
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var user = new User();
                            user.Id = dr.GetFieldValue<int>("id");
                            user.Username = dr.GetFieldValue<string>("username");
                            user.Email = dr.GetFieldValue<string>("email");
                            user.Name = dr.GetFieldValue<string>("name");
                            user.Surname = dr.GetFieldValue<string>("surname");
                            user.Password = dr.GetFieldValue<string>("password");
                            user.Salt = Convert.FromBase64String(dr.GetFieldValue<string>("salt"));
                            users.Add(user);
                        }
                    }

                }
                return users;
            }
            catch (Exception ex)
            {
                throw;
                //log exception
            }
        }


        public User GetByUsername(string username)
        {
            var user = new User();
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using (var cmd = new NpgsqlCommand(@"SELECT * FROM users WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            user.Id = dr.GetFieldValue<int>("id");
                            user.Username = dr.GetFieldValue<string>("username");
                            user.Email = dr.GetFieldValue<string>("email");
                            user.Name = dr.GetFieldValue<string>("name");
                            user.Surname = dr.GetFieldValue<string>("surname");
                            user.Password = dr.GetFieldValue<string>("password");
                            user.Salt = Convert.FromBase64String(dr.GetFieldValue<string>("salt"));
                        }
                    }

                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
                //log exception
            }
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }


    }
}