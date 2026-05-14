using MediatR;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Commands
{
    public record UpdateTeacherCommand(
        int Id,
        string Fullname,
        decimal IndividualLessonsShare,
        string? Notes) : IRequest;
}
