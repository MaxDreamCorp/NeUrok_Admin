using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.GroupOperation.Queries
{
    public record GetAllGroupsQuery() : IRequest<List<GroupDTO>>;
}
