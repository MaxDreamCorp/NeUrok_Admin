using MediatR;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Commands
{
    public record UpdateSubscriptionCommand(
        int Id,
        string Name,
        int ClassesTypeId,
        decimal Cost,
        int ClassesAmount) : IRequest;
}
