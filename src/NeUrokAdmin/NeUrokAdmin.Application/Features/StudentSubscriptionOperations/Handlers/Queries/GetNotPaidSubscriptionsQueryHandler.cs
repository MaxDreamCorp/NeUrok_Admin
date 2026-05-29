using MediatR;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Handlers.Queries
{
    public class GetNotPaidSubscriptionsQueryHandler : IRequestHandler<GetNotPaidSubscriptionsQuery, List<ExpiringSubscriptionDTO>>
    {
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public GetNotPaidSubscriptionsQueryHandler(IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task<List<ExpiringSubscriptionDTO>> Handle(GetNotPaidSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subs = await _studentSubscriptionRepository.GetNotPaidAsync(cancellationToken);

            return subs.Select(s => new ExpiringSubscriptionDTO(
                s.Id,
                s.StudentId,
                s.Student.Client.ChildFullname,
                s.SubscriptionStartDate,
                s.SubscriptionFinishDate,
                s.Course.Name,
                s.ClassesAmount,
                s.IsPaid == 1)).ToList();
        }
    }
}
