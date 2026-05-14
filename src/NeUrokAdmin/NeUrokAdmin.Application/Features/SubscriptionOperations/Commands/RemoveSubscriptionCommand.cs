using MediatR;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Commands
{
    public record RemoveSubscriptionCommand(
        int Id) : IRequest;

}
