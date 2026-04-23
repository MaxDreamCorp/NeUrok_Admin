using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class ClassType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Subscribtion> Subscribtions { get; set; } = new List<Subscribtion>();
}
