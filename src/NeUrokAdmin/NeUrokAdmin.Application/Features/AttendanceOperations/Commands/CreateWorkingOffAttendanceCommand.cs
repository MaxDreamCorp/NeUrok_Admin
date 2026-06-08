using MediatR;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Commands
{
    public record CreateWorkingOffAttendanceCommand(
        int StudentId,
        DateTime OldDatetime,
        DateTime WorkOffDatetime,
        int CourseId,
        int ClassesTypeId,
        int TeacherId,
        int? GroupId) : IRequest;
}
