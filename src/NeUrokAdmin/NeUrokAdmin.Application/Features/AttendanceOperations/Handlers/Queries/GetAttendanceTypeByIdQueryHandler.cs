using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Handlers.Queries
{
    public class GetAttendanceTypeByIdQueryHandler : IRequestHandler<GetAttendanceTypeByIdQuery, AttendanceTypeDTO?>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public GetAttendanceTypeByIdQueryHandler(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task<AttendanceTypeDTO?> Handle(GetAttendanceTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var status = await _attendanceTypeRepository.GetByIdAsync(request.Id, cancellationToken);
            return status != null
                ? new AttendanceTypeDTO(
                    status.Id,
                    status.Type)
                : null;
        }
    }
}
