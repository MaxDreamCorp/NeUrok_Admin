using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
