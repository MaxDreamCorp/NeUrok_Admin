using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Queries
{
    public record GetAllSubscriptionsQuery() : IRequest<List<SubscriptionDTO>>;
}
