using MediatR;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;

namespace NeUrokAdmin.Application.Features.ClientOperations.Queries
{
    public record GetClientsByFilterQuery(ClientSearchDTO Request) : IRequest<List<ClientDTO>>;
}
