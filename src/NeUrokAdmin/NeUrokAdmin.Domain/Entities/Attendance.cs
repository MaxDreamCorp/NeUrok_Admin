namespace NeUrokAdmin.Domain.Entities;

public partial class Attendance
{


    public int Id { get; set; }

    public int? ClientId { get; set; }

    public DateTime Datetime { get; set; }

    public int CourseId { get; set; }

    public int ClassTypeId { get; set; }

    public int TeacherId { get; set; }

    public int? GroupId { get; set; }

    public sbyte IsCompleted { get; set; }

    public int? AttendanceStatusId { get; set; }

    public int AttendanceTypeId { get; set; }

    public decimal? Price { get; set; }

    public decimal? TeacherShare { get; set; }

    public virtual AttendanceStatus? AttendanceStatus { get; set; } = null!;

    public virtual AttendanceType AttendanceType { get; set; } = null!;

    public virtual ClassType ClassType { get; set; } = null!;

    public virtual Client? Client { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Group? Group { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;

    private Attendance() { }

    public static Attendance Create(int id,
                          int? clientId,
                          DateTime dateTime,
                          int courseId,
                          int classTypeId,
                          int teacherId,
                          int? groupId,
                          sbyte isCompleted,
                          int? attendanceStatusId,
                          int attendanceTypeId,
                          decimal? price,
                          decimal? teacherShare)
    {
        return new Attendance
        {
            Id = id,
            ClientId = clientId,
            Datetime = dateTime,
            CourseId = courseId,
            ClassTypeId = classTypeId,
            TeacherId = teacherId,
            GroupId = groupId,
            IsCompleted = isCompleted,
            AttendanceStatusId = attendanceStatusId,
            AttendanceTypeId = attendanceTypeId,
            Price = price,
            TeacherShare = teacherShare
        };
    }
}
