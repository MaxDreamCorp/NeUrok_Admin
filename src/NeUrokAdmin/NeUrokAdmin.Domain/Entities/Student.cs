using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
