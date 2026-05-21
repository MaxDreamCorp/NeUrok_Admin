namespace NeUrokAdmin.Domain.DTOs
{
    public record AttendanceDTO(
        int ClientId,
        DateTime Datetime,
        CourseDTO Course,
        ClassesTypeDTO ClassesType,
        TeacherDTO Teacher,
        int? GroupId,
        bool IsComplited,
        AttendanceStatusDTO? AttendanceStatus,
        AttendanceTypeDTO AttendanceType,
        decimal? Price,
        decimal? TeacherShare);
}
