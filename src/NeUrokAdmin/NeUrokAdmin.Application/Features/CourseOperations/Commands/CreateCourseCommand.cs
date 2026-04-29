using MediatR;

namespace NeUrokAdmin.Application.Features.CourseOperations.Commands
{
    public record CreateCourseCommand(string Name) : IRequest;
}
