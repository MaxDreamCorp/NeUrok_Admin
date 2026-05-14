using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Queries
{
    public record GetAllSubscriptionStatusesQuery() : IRequest<List<SubscriptionStatusDTO>>;
}
