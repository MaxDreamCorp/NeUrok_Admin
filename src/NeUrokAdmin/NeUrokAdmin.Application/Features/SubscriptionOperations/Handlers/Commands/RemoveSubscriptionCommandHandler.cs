using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Commands;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Commands
{
    public class RemoveSubscriptionCommandHandler : IRequestHandler<RemoveSubscriptionCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public RemoveSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task Handle(RemoveSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (subscription == null)
                throw new Exception("Данного абонемента не существует");

            await _subscriptionRepository.RemoveAsync(subscription, cancellationToken);
        }
    }
}
