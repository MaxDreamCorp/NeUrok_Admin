using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.CourseOperations.Queries
{
    public record GetAllCoursesQuery() : IRequest<List<CourseDTO>>;
}
