using MediatR;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Handlers.Queries
{
    public class GetStudentSubscriptionByIdQueryHandler : IRequestHandler<GetStudentSubscriptionByIdQuery, StudentSubscriptionDTO?>
    {
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;
        private readonly GettingService _gettingService;

        public GetStudentSubscriptionByIdQueryHandler(IStudentSubscriptionRepository studentSubscriptionRepository, GettingService gettingService)
        {
            _studentSubscriptionRepository = studentSubscriptionRepository;
            _gettingService = gettingService;
        }

        public async Task<StudentSubscriptionDTO?> Handle(GetStudentSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var studentSubscription = await _studentSubscriptionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (studentSubscription == null)
                return null;

            return _gettingService.GetStudentSubscriptionDTOFromStudentSubscription(studentSubscription, cancellationToken);
        }
    }
}
