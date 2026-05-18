using MediatR;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Commands
{
    public record CreateAttendancesForGroupCommand(
        int GroupId,
        int ClassesTypeId) : IRequest;
}
