using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries
{
    public record GetNotPaidSubscriptionsQuery() : IRequest<List<ExpiringSubscriptionDTO>>;
}
