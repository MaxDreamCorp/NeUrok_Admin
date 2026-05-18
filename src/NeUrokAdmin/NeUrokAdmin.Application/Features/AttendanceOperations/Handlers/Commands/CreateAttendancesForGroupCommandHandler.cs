using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Enums;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Handlers.Commands
{
    public class CreateAttendancesForGroupCommandHandler : IRequestHandler<CreateAttendancesForGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IClassesTypeRepository _classesTypeRepository;

        public CreateAttendancesForGroupCommandHandler(IGroupRepository groupRepository, IAttendanceRepository attendanceRepository, IClassesTypeRepository classesTypeRepository)
        {
            _groupRepository = groupRepository;
            _attendanceRepository = attendanceRepository;
            _classesTypeRepository = classesTypeRepository;
        }

        public async Task Handle(CreateAttendancesForGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new ArgumentNullException("Данной не существует");

            var classesType = await _classesTypeRepository.GetByIdAsync(request.ClassesTypeId, cancellationToken);
            if (classesType == null)
                throw new ArgumentNullException("Данной не существует");

            foreach (var groupDate in group.GroupDates)
            {
                foreach (var student in group.Students)
                {
                    int id = await _attendanceRepository.GetNextIdAsync(cancellationToken);
                    var attendance = Attendance.Create(
                        id,
                        student.ClientId,
                        groupDate.Datetime,
                        group.CourseId,
                        classesType.Id,
                        group.TeacherId,
                        group.Id,
                        0,
                        null,
                        (int)AttendanceTypeEnum.Standart,
                        null,
                        null);
                    await _attendanceRepository.AddAsync(attendance, cancellationToken);
                    id++;
                }
            }
        }
    }
}
