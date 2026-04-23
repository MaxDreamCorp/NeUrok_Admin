using System.Security.Cryptography;
using NeUrokAdmin.Domain.Interfaces;

namespace NeUrokAdmin.Infrastructure.Services.Security
{
    public class SHA512Hasher : IHasher
    {
        public (byte[] hash, byte[] salt) Hash(string password, byte[]? salt = null)
        {
            if (salt is null)
                salt = SaltGenerator.Generate(32);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA512))
            {
                return (pbkdf2.GetBytes(256), salt);
            }
        }

        public bool Verify(string enteredPassword, byte[] storedPassword, byte[] storedSalt)
        {
            var hashedEnteredPassword = Hash(enteredPassword, storedSalt);
            return CryptographicOperations.FixedTimeEquals(storedPassword, hashedEnteredPassword.hash);
        }
    }
}
