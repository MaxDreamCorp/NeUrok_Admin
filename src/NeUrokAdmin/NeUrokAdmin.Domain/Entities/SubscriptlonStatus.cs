using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class SubscriptlonStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();
}
