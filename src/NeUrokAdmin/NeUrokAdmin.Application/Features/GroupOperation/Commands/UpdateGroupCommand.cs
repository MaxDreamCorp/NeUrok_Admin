using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.GroupOperation.Commands
{
    public record UpdateGroupCommand(
        int Id,
        string Name,
        int CourseId,
        int TeacherId,
        int GroupStatusId,
        string WeekDays,
        TimeOnly Time,
        List<DateTime> Dates,
        List<StudentDTO> Students) : IRequest;
}
