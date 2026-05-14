using MediatR;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Commands
{
    public record RemoveTeacherCommand(int Id) : IRequest;
}
