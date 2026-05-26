using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries
{
    public record GetAllSubscriptionStatusesQuery() : IRequest<List<SubscriptionStatusDTO>>;
}
