using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class GroupDate
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public DateTime Datetime { get; set; }

    public virtual Group Group { get; set; } = null!;
}
