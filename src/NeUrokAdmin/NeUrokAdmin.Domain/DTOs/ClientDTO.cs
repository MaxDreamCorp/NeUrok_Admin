namespace NeUrokAdmin.Domain.DTOs
{
    public record ClientDTO(
        int Id,
        string ChildFullname,
        DateOnly? BirthDate,
        DateOnly RegistrationDate,
        int? Grade,
        ClientStatusDTO Status,
        string ParentName,
        string Phone,
        List<CourseDTO>? WishedCourses,
        string? Notes,
        string? AdditionalPhones)
    {
        public string WishedCoursesDisplay => WishedCourses != null && WishedCourses.Any()
            ? string.Join(", ", WishedCourses.Select(c => c.Name))
            : string.Empty;
    }
}
