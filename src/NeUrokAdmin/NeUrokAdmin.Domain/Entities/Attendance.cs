using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class Attendance
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int CourseId { get; set; }

    public int ClassTypeId { get; set; }

    public int TeacherId { get; set; }

    public int? GroupId { get; set; }

    public sbyte IsCompleted { get; set; }

    public int AttendanceStatusId { get; set; }

    public int AttendanceTypeId { get; set; }

    public decimal? Price { get; set; }

    public decimal? TeacherShare { get; set; }

    public virtual AttendanceStatus AttendanceStatus { get; set; } = null!;

    public virtual AttendanceType AttendanceType { get; set; } = null!;

    public virtual ClassType ClassType { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Group? Group { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}
