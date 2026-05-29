using MediatR;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Handlers.Queries
{
    public class GetExpiringSubscriptionsQueryHandler : IRequestHandler<GetExpiringSubscriptionsQuery, List<ExpiringSubscriptionDTO>>
    {
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public GetExpiringSubscriptionsQueryHandler(IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task<List<ExpiringSubscriptionDTO>> Handle(GetExpiringSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subs = await _studentSubscriptionRepository.GetExpiringAsync(cancellationToken);

            return subs.Select(s => new ExpiringSubscriptionDTO(
                s.Student.Client.ChildFullname,
                s.SubscriptionFinishDate,
                s.Course.Name,
                s.ClassesAmount,
                s.IsPaid == 1)).ToList();
        }
    }
}
