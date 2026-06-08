using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Queries
{
    public record GetAttendanceTypeByIdQuery(int Id) : IRequest<AttendanceTypeDTO?>;
}
