using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class AttendanceStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
