using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.GroupOperation.Commands
{
    public record GetGroupStudentsAttendancesCommand(int GroupId) : IRequest<List<StudentAttendancesDTO>>;
}
