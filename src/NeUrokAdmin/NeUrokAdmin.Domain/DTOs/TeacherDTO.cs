namespace NeUrokAdmin.Domain.DTOs
{
    public record TeacherDTO(
        int Id,
        string Fullname,
        decimal IndividualLessonsShare,
        string? Notes);
}
