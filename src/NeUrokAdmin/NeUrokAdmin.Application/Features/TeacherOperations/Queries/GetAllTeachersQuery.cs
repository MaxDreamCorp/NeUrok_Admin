using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Queries
{
    public record GetAllTeachersQuery() : IRequest<List<TeacherDTO>>;
}
