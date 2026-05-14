using MediatR;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Queries
{
    public record GetSubscriptionsByFilterQuery(SubscriptionSearchDTO Request) : IRequest<List<SubscriptionDTO>>;
}
