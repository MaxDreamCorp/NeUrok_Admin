namespace NeUrokAdmin.Domain.Interfaces
{
    public interface IHasher
    {
        (byte[] hash, byte[] salt) Hash(string password, byte[]? salt = null);
        bool Verify(string enteredPassword, byte[] storedPassword, byte[] storedSalt);
    }
}
