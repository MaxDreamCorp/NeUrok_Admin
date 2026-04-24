using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.ClientOperations.Queries
{
    public record GetAllClientsQuery() : IRequest<List<ClientDTO>>;
}
