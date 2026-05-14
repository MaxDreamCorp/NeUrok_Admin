using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Queries
{
    public class GetAllSubscriptionStatusesQueryHandler : IRequestHandler<GetAllSubscriptionStatusesQuery, List<SubscriptionStatusDTO>>
    {
        private readonly ISubscriptionStatusRepository _subscriptionStatusRepository;

        public GetAllSubscriptionStatusesQueryHandler(ISubscriptionStatusRepository subscriptionStatusRepository)
        {
            _subscriptionStatusRepository = subscriptionStatusRepository;
        }

        public async Task<List<SubscriptionStatusDTO>> Handle(GetAllSubscriptionStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _subscriptionStatusRepository.GetAllAsync(cancellationToken);

            return statuses.Select(ss => new SubscriptionStatusDTO(
                ss.Id,
                ss.Status))
                .ToList();
        }
    }
}
