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

    private Client() { }

    public static Client Create(int id,
        string childFullname,
        DateOnly? birthDate,
        DateOnly registrationDate,
        int grade,
        int statusId,
        string parentName,
        string phone,
        List<Course> wishedCourses,
        string? additionalPhones,
        string? notes)
    {
        return new Client
        {
            Id = id,
            ChildFullname = childFullname,
            BirthDate = birthDate,
            RegistrationDate = registrationDate,
            Grade = grade,
            StatusId = statusId,
            ParentName = parentName,
            Phone = phone,
            Courses = wishedCourses,
            AdditionalPhones = additionalPhones,
            Notes = notes
        };
    }
}
