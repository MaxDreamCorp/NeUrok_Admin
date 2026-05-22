using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Handlers.Queries
{
    public class GetAttendanceStatusesQueryHandler : IRequestHandler<GetAttendanceStatusesQuery, List<AttendanceStatusDTO>>
    {
        private readonly IAttendanceStatusRepository _attendanceStatusRepository;

        public GetAttendanceStatusesQueryHandler(IAttendanceStatusRepository attendanceStatusRepository)
        {
            _attendanceStatusRepository = attendanceStatusRepository;
        }

        public async Task<List<AttendanceStatusDTO>> Handle(GetAttendanceStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _attendanceStatusRepository.GetAllAsync(cancellationToken);

            return statuses.Select(s => new AttendanceStatusDTO(
                s.Id,
                s.Status))
                .ToList();
        }
    }
}
