using System.Security.Cryptography;
using napper_be.Models;
using napper_be.Entities;
using napper_be.Repository;
using napper_be.Helpers;

namespace napper_be.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserStorage _storage;
        public RegisterService(IUserStorage storage)
        {
            _storage = storage;
        }


        public bool Register(User user)
        {
            if (Validate())
            {
                //Create the salt value with a cryptographic PRNG:
                new RNGCryptoServiceProvider().GetBytes(user.Salt = new byte[16]);
                user.HashedPassword = PasswordUtils.HashPassword(user.Salt, user.Password);
                _storage.Create(user);
                return true;
            }
            return false;
        }


        private bool Validate()
        {
            return true;
        }

    }

}