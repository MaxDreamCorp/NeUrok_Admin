using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
