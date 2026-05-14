using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentOperations.Queries
{
    public record GetAllStudentsQuery() : IRequest<List<StudentDTO>>;
}
