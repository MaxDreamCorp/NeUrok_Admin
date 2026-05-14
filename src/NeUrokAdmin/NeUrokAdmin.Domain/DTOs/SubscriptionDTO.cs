namespace NeUrokAdmin.Domain.DTOs
{
    public record SubscriptionDTO(
        int Id,
        string Name,
        ClassesTypeDTO ClassesType,
        decimal Cost,
        int ClassesAmount);
}
