using System;
using System.Security.Cryptography;

namespace napper_be
{
    public class User
    {
        private int _Id;
        private string _name;
        private string _surname;
        private string _username;
        private string _password;
        private string _hashedPassword;
        private string _email;
        private byte[] _salt;

        public int Id { get => _Id; set => _Id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public string HashedPassword { get => _hashedPassword; set => _hashedPassword = value; }
        public string Email { get => _email; set => _email = value; }
        public byte[] Salt { get => _salt; set => _salt = value; }



        private readonly IUserStorage _storage;

        public User()
        {
            
        }
        public User(IUserStorage storage)
        {
            _storage = storage;
        }

        public User(int userId, string username, string name, string surname, string password, byte[] salt, string email, IUserStorage storage)
        {
            _Id = userId;
            _name = name;
            _surname = surname;
            _username = username;
            _hashedPassword = password;
            _email = email;
            _storage = storage;
            _salt = salt;
        }

        public int Login(string username, string password)
        {
            var user = _storage.GetByUsername(username);
            var hashedPassword = PasswordUtils.HashPassword(user.Salt, password);
            if (hashedPassword == user.HashedPassword)
            {
                return user.Id;
            }
            else
            {
                return 0;
            }
        }

        public bool Register(User user)
        {
            if(user.Validate())
            {
                //Create the salt value with a cryptographic PRNG:
                new RNGCryptoServiceProvider().GetBytes(user.Salt = new byte[16]);
                user.HashedPassword = PasswordUtils.HashPassword(user.Salt, user.Password);
                _storage.Create(user);
                return true;
            }
            return false;
        }

        public User GetUser(string username)
        {
            return _storage.GetByUsername(username);
        }

        private bool Validate()
        {
            return true;
        }
    }
}