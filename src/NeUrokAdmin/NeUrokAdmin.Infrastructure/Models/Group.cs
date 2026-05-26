using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Infrastructure.Models;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public int GroupStatusId { get; set; }

    public string WeekDays { get; set; } = null!;

    public TimeOnly Time { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<GroupDate> GroupDates { get; set; } = new List<GroupDate>();

    public virtual GroupStatus GroupStatus { get; set; } = null!;

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual Teacher Teacher { get; set; } = null!;
}
