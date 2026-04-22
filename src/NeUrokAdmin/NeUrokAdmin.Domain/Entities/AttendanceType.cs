using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class AttendanceType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
