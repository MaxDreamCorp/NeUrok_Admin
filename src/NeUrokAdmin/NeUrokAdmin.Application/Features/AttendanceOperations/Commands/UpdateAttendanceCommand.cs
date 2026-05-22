using MediatR;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Commands
{
    public record UpdateAttendanceCommand(
        int Id,
        bool IsComplited,
        int AttendanceStatusId,
        decimal Price,
        decimal TeacherShare) : IRequest;
}
