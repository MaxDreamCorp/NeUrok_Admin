namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentDTO(
        int Id,
        ClientDTO Client,
        List<StudentSubscriptionDTO> StudentSubscriptions);
}
