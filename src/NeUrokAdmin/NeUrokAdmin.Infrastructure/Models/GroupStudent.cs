using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class GroupStudent
{
    public int GroupId { get; set; }

    public int StudentId { get; set; }

    public int ActiveSubscriptionId { get; set; }

    public virtual StudentSubscription ActiveSubscription { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
