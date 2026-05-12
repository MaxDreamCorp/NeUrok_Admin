namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentSubscriptionDTO(
        int Id,
        int StudentId,
        SubscriptionDTO Subscription,
        bool IsPaid,
        CourseDTO Course,
        SubscriptionStatusDTO SubscriptionStatus,
        DateOnly StartDate,
        DateOnly FinishDate);
}
