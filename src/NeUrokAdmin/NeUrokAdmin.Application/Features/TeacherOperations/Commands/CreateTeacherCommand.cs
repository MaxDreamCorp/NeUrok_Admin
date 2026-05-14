using MediatR;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Commands
{
    public record CreateTeacherCommand(
        string Fullname,
        decimal IndividualLessonsShare,
        string? Notes) : IRequest;
}
