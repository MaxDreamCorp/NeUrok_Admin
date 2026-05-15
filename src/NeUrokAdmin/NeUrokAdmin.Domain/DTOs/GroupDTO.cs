namespace NeUrokAdmin.Domain.DTOs
{
    public record GroupDTO(
        int Id,
        string Name,
        CourseDTO Course,
        TeacherDTO Teacher,
        GroupStatusDTO GroupStatus,
        string WeekDays,
        TimeOnly Time,
        List<DateTime> Dates,
        List<StudentDTO> Students);
}
