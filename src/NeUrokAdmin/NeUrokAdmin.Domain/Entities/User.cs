namespace NeUrokAdmin.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    private User() { }

    public static User Create(int id,
                              string login,
                              byte[] passwordHash,
                              byte[] passwordSalt) =>
        new User()
        {
            Id = id,
            Login = login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
}
