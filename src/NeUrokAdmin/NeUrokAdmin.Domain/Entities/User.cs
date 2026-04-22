using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;
}
