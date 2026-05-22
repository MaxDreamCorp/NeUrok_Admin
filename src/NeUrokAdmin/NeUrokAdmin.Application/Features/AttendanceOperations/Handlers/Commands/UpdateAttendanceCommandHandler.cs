using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Handlers.Commands
{
    public class UpdateAttendanceCommandHandler : IRequestHandler<UpdateAttendanceCommand>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceStatusRepository _attendanceStatusRepository;

        public UpdateAttendanceCommandHandler(IAttendanceRepository attendanceRepository, IAttendanceStatusRepository attendanceStatusRepository)
        {
            _attendanceRepository = attendanceRepository;
            _attendanceStatusRepository = attendanceStatusRepository;
        }

        public async Task Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(request.Id, cancellationToken);
            if (attendance == null) throw new KeyNotFoundException("Посещение не найдена");

            var attendanceStatus = await _attendanceStatusRepository.GetByIdAsync(request.AttendanceStatusId, cancellationToken);
            if (attendanceStatus == null) throw new ArgumentNullException("Такого статуса не существует");

            if (request.IsComplited)
            {
                attendance.IsCompleted = 1;
                attendance.AttendanceStatusId = attendanceStatus.Id;
                attendance.Price = request.Price;
                attendance.TeacherShare = request.TeacherShare;
            }
            else
            {
                attendance.IsCompleted = 0;
                attendance.AttendanceStatusId = null;
                attendance.Price = null;
                attendance.TeacherShare = null;
            }
            await _attendanceRepository.UpdateAsync(attendance, cancellationToken);
        }
    }
}
