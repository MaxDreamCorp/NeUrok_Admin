namespace NeUrokAdmin.Domain.DTOs
{
    public record ExpiringSubscriptionDTO(
        int SubscriptionId,
        int StudentId,
        string ClientFullname,
        DateOnly StartDate,
        DateOnly FinishDate,
        string CourseName,
        int ClassesAmount,
        bool IsPaid);
}
