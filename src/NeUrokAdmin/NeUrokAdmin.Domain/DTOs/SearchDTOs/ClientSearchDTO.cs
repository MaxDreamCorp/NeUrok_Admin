namespace NeUrokAdmin.Domain.DTOs.SearchDTOs
{
    public record ClientSearchDTO(
        int? Id = null,
        string? ChildFullname = null,
        string? ParentName = null,
        string? Phone = null,
        string? AdditionalPhone = null,
        string? Status = null,
        string? Notes = null,
        int? IdFrom = null,
        int? IdTo = null,
        int? Grade = null,
        int? GradeFrom = null,
        int? GradeTo = null,
        DateOnly? BirthDate = null,
        DateOnly? BirthDateFrom = null,
        DateOnly? BirthDateTo = null,
        int? BirthDateDay = null,
        int? BirthDateMonth = null,
        int? BirthDateYear = null,
        DateOnly? RegistrationDate = null,
        DateOnly? RegistrationDateFrom = null,
        DateOnly? RegistrationDateTo = null,
        int? RegistrationDateDay = null,
        int? RegistrationDateMonth = null,
        int? RegistrationDateYear = null,
        List<int>? WishedCourseIds = null,
        List<int>? StatusIds = null,
        List<CourseDTO>? WishedCourses = null,
        List<ClientStatusDTO>? Statuses = null
    );
}
