using System;
using System.Collections.Generic;

namespace NeUrokAdmin.Domain.Entities;

public partial class Client
{
    public int Id { get; set; }

    public string ChildFullname { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public int StatusId { get; set; }

    public int Grade { get; set; }

    public string ParentName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? AdditionalPhones { get; set; }

    public string? Notes { get; set; }

    public virtual ClientStatus Status { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
