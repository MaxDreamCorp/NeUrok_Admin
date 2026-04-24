using System.Security.Cryptography;

namespace NeUrokAdmin.Infrastructure.Services.Security
{
    public static class SaltGenerator
    {
        public static byte[] Generate(int length)
            => RandomNumberGenerator.GetBytes(length);
    }
}
