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
        Dictionary<StudentDTO, StudentSubscriptionDTO> StudentAndSubscription)
    {
        public List<StudentDTO> Students { get => StudentAndSubscription.Keys.ToList(); }
        public int StudentCount { get => StudentAndSubscription.Count; }
        public int ClassesCount { get => Dates.Count; }
    }
}
