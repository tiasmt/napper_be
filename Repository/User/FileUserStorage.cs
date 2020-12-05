using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using napper_be.Exceptions;

namespace napper_be
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
            using (var file = new StreamWriter(File.Create(_directoryPath + user.Username + ".usr")))
            {
                file.WriteLine($"{UserId}{user.Id}");
                file.WriteLine($"{Username}{user.Username}");
                file.WriteLine($"{Name}{user.Name}");
                file.WriteLine($"{Surname}{user.Surname}");
                file.WriteLine($"{Password}{user.Password}");
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
                    var userIdFromFile = int.Parse(reader.ReadLine().Substring(UserId.Length));
                    var usernameFromFile = reader.ReadLine().Substring(Username.Length);
                    var nameFromFile = reader.ReadLine().Substring(Name.Length);
                    var surnameFromFile = reader.ReadLine().Substring(Surname.Length);
                    var passwordFromFile = reader.ReadLine().Substring(Password.Length);
                    var saltFromFile = Convert.FromBase64String(reader.ReadLine().Substring(Salt.Length));
                    var emailFromFile = reader.ReadLine().Substring(Email.Length);
                    users.Add(new User(userIdFromFile, usernameFromFile, nameFromFile, surnameFromFile, passwordFromFile, saltFromFile, emailFromFile, this));
                }
            }
            return users;
        }


        public User GetByUsername(string username)
        {
            User user;
            try
            {
                var fileStream = new FileStream(_directoryPath + username + ".usr", FileMode.Open);
                using (var reader = new StreamReader(fileStream))
                {
                    var userIdFromFile = int.Parse(reader.ReadLine().Substring(UserId.Length));
                    var usernameFromFile = reader.ReadLine().Substring(Username.Length);
                    var nameFromFile = reader.ReadLine().Substring(Name.Length);
                    var surnameFromFile = reader.ReadLine().Substring(Surname.Length);
                    var passwordFromFile = reader.ReadLine().Substring(Password.Length);
                    var saltFromFile = Convert.FromBase64String(reader.ReadLine().Substring(Salt.Length));
                    var emailFromFile = reader.ReadLine().Substring(Email.Length);
                    user = new User(userIdFromFile, usernameFromFile, nameFromFile, surnameFromFile, passwordFromFile, saltFromFile, emailFromFile, this);
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