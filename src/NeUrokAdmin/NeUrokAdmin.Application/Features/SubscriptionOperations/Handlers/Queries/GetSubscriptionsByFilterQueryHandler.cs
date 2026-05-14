using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Queries
{
    public class GetSubscriptionsByFilterQueryHandler : IRequestHandler<GetSubscriptionsByFilterQuery, List<SubscriptionDTO>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetSubscriptionsByFilterQueryHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<SubscriptionDTO>> Handle(GetSubscriptionsByFilterQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _subscriptionRepository.SearchAsync(request.Request, cancellationToken);

            return subscriptions.Select(s => new SubscriptionDTO(
                s.Id,
                s.Name,
                new(
                    s.ClassesType.Id,
                    s.ClassesType.Type),
                s.Cost,
                s.ClassesAmount)).ToList();
        }
    }
}
