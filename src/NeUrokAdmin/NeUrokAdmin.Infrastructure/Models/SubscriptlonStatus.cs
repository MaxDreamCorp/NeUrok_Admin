using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class SubscriptlonStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();
}
