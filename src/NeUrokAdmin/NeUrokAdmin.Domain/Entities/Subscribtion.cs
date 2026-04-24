using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class Subscribtion
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ClassesTypeId { get; set; }

    public decimal Cost { get; set; }

    public int ClassesAmount { get; set; }

    public virtual ClassType ClassesType { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();
}
