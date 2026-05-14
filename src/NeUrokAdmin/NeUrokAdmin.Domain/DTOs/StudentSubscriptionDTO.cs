namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentSubscriptionDTO(
        int Id,
        int StudentId,
        ClassesTypeDTO ClassesType,
        decimal Cost,
        int ClassesAmount,
        bool IsPaid,
        CourseDTO Course,
        SubscriptionStatusDTO SubscriptionStatus,
        DateOnly StartDate,
        DateOnly FinishDate);
}
