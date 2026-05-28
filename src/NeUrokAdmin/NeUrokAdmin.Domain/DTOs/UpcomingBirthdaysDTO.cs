namespace NeUrokAdmin.Domain.DTOs
{
    public record UpcomingBirthdaysDTO(
        Dictionary<string, string> TodayBirthdays,
        Dictionary<string, string> TomorrowBirthdays,
        Dictionary<string, string> PostTomorrowBirthdays);
}
