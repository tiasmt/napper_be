using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using napper_be.Exceptions;
using napper_be.Models;

namespace napper_be.Repository
{
    public class FileUserStorage : IUserStorage
    {
        private const string UserId = "UserId:";
        private const string Username = "Username:";
        private const string Name = "Username:";
        private const string Surname = "Surname:";
        private const string Password = "Password:";
        private const string Salt = "Salt:";
        private const string Email = "Email:";

        private readonly string _directoryPath;

        public FileUserStorage(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Create(User user)
        {
            user.Id = Directory.EnumerateFiles(_directoryPath, "*.usr", SearchOption.AllDirectories).Count() + 1; 
            using (var file = new StreamWriter(File.Create(_directoryPath + user.Username + ".usr")))
            {
                file.WriteLine($"{UserId}{user.Id}");
                file.WriteLine($"{Username}{user.Username}");
                file.WriteLine($"{Name}{user.Name}");
                file.WriteLine($"{Surname}{user.Surname}");
                file.WriteLine($"{Password}{user.HashedPassword}");
                file.WriteLine($"{Salt}{Convert.ToBase64String(user.Salt)}");
                file.WriteLine($"{Email}{user.Email}");
            }
        }

        public void Delete(string username)
        {
            File.Delete(_directoryPath + username + ".usr");
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            foreach (var file in Directory.EnumerateFiles(_directoryPath))
            {
                var fileStream = new FileStream(file, FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    var user = new User();
                    user.Id = int.Parse(reader.ReadLine().Substring(UserId.Length));
                    user.Username = reader.ReadLine().Substring(Username.Length);
                    user.Name = reader.ReadLine().Substring(Name.Length);
                    user.Surname = reader.ReadLine().Substring(Surname.Length);
                    user.HashedPassword = reader.ReadLine().Substring(Password.Length);
                    user.Salt = Convert.FromBase64String(reader.ReadLine().Substring(Salt.Length));
                    user.Email = reader.ReadLine().Substring(Email.Length);
                    users.Add(user);
                }
            }
            return users;
        }


        public User GetByUsername(string username)
        {
            User user = new User();
            try
            {
                var fileStream = new FileStream(_directoryPath + username + ".usr", FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    user.Id = int.Parse(reader.ReadLine().Substring(UserId.Length));
                    user.Username = reader.ReadLine().Substring(Username.Length);
                    user.Name = reader.ReadLine().Substring(Name.Length);
                    user.Surname = reader.ReadLine().Substring(Surname.Length);
                    user.HashedPassword = reader.ReadLine().Substring(Password.Length);
                    user.Salt = Convert.FromBase64String(reader.ReadLine().Substring(Salt.Length));
                    user.Email = reader.ReadLine().Substring(Email.Length);
                    return user;
                }
            }
            catch
            {
                throw new NotFoundException($"Invalid username / password.");
            }
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }


    }
}