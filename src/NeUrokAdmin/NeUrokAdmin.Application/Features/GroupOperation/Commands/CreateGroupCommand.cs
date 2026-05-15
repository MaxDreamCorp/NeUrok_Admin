using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.GroupOperation.Commands
{
    public record CreateGroupCommand(
        string Name,
        int CourseId,
        int TeacherId,
        int GroupStatusId,
        string WeekDays,
        TimeOnly Time,
        List<DateTime> Dates,
        List<StudentDTO> Students) : IRequest;
}
