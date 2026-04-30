using MediatR;

namespace NeUrokAdmin.Application.Features.CourseOperations.Commands
{
    public record RemoveCourseCommand(int Id) : IRequest;
}
