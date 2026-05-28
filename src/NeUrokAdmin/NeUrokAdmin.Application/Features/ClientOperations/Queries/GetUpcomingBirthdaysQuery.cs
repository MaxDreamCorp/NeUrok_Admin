using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.ClientOperations.Queries
{
    public record GetUpcomingBirthdaysQuery() : IRequest<UpcomingBirthdaysDTO>;
}
