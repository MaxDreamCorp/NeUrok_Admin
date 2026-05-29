namespace NeUrokAdmin.Domain.DTOs
{
    public record ExpiringSubscriptionDTO(
        string ClientFullname,
        DateOnly FinishDate,
        string CourseName,
        int ClassesAmount,
        bool IsPaid);
}
