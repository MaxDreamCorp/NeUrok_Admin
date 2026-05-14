using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Queries
{
    public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, List<SubscriptionDTO>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetAllSubscriptionsQueryHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<SubscriptionDTO>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subs = await _subscriptionRepository.GetAllAsync(cancellationToken);
            return subs.Select(s => new SubscriptionDTO(
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
