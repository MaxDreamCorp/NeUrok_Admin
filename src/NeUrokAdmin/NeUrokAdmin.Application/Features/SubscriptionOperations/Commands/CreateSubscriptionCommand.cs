using MediatR;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Commands
{
    public record CreateSubscriptionCommand(
        string Name,
        int ClassesTypeId,
        decimal Cost,
        int ClassesAmount) : IRequest;
}
