using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.GroupOperation.Queries
{
    public record GetAllGroupStatusesQuery() : IRequest<List<GroupStatusDTO>>;
}
