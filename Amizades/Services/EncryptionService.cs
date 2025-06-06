using Amizades.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Amizades.Services
{
    public class EncryptionService
    {
        public string Encrypt<T>(string password) where T : class, new()
        {
            var passwordHasher = new PasswordHasher<T>();
            var dummyUser = new T();
            return passwordHasher.HashPassword(dummyUser, password);
        }

        public bool Verify<T>(string inputPassword, string userPassword) where T : class, new()
        {
            var passwordHasher = new PasswordHasher<T>();
            var dummyUser = new T();
            var resultado = passwordHasher.VerifyHashedPassword(dummyUser, userPassword, inputPassword);

            return resultado == PasswordVerificationResult.Success;
        }
    }
}
