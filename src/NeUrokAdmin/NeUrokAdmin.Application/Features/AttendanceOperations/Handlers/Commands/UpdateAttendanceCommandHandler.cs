using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Domain.Entities;
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
            if (attendanceStatus == null && request.AttendanceStatusId != 0) throw new ArgumentNullException("Такого статуса не существует");

            if (request.IsComplited && attendanceStatus != null)
            {
                attendance.IsCompleted = 1;
                attendance.AttendanceStatusId = attendanceStatus.Id;
                attendance.Price = request.Price;
                attendance.TeacherShare = request.TeacherShare;
                attendance.AbsentCause = request.AbsentCause;
                attendance.Notes = request.Notes;
                await _attendanceRepository.UpdateAsync(attendance, cancellationToken);
            }
            else
            {
                //await _attendanceRepository.RemoveAsync(attendance, cancellationToken);
                var newAttendance = Attendance.Create(
                    attendance.Id,
                    attendance.ClientId,
                    attendance.Datetime,
                    attendance.CourseId,
                    attendance.ClassTypeId,
                    attendance.TeacherId,
                    attendance.GroupId,
                    0,
                    null,
                    attendance.AttendanceTypeId,
                    null,
                    null,
                    null,
                    attendance.Notes);
                await _attendanceRepository.UpdateAsync(newAttendance, cancellationToken);
            }

        }
    }
}
