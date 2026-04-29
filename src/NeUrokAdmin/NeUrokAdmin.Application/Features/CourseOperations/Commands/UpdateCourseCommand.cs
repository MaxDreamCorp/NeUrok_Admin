using MediatR;

namespace NeUrokAdmin.Application.Features.CourseOperations.Commands
{
    public record UpdateCourseCommand(int Id,
        string Name) : IRequest;
}
