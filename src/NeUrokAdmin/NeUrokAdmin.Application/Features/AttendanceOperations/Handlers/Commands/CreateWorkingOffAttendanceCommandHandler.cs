using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Enums;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.AttendanceOperations.Handlers.Commands
{
    public class CreateWorkingOffAttendanceCommandHandler : IRequestHandler<CreateWorkingOffAttendanceCommand>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IClassesTypeRepository _classesTypeRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupDateRepository _groupDateRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public CreateWorkingOffAttendanceCommandHandler(IAttendanceRepository attendanceRepository,
                                                        ICourseRepository courseRepository,
                                                        IClassesTypeRepository classesTypeRepository,
                                                        ITeacherRepository teacherRepository,
                                                        IAttendanceTypeRepository attendanceTypeRepository,
                                                        IGroupRepository groupRepository,
                                                        IGroupDateRepository groupDateRepository,
                                                        IStudentRepository studentRepository,
                                                        IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _attendanceRepository = attendanceRepository;
            _courseRepository = courseRepository;
            _classesTypeRepository = classesTypeRepository;
            _teacherRepository = teacherRepository;
            _attendanceTypeRepository = attendanceTypeRepository;
            _groupRepository = groupRepository;
            _groupDateRepository = groupDateRepository;
            _studentRepository = studentRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task Handle(CreateWorkingOffAttendanceCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
            if (student == null)
                throw new ArgumentNullException("Данного ученика не существует");

            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course == null)
                throw new ArgumentNullException("Данного курса существует");

            var classesType = await _classesTypeRepository.GetByIdAsync(request.ClassesTypeId, cancellationToken);
            if (classesType == null)
                throw new ArgumentNullException("Данного типа занятий существует");

            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
            if (teacher == null)
                throw new ArgumentNullException("Данного преподавателя не существует");

            Group? group = null;
            if (request.GroupId.HasValue)
            {
                group = await _groupRepository.GetByIdAsync(request.GroupId.Value, cancellationToken);
                if (group == null)
                    throw new ArgumentNullException("Данной группы не существует");
            }

            var attendance = Attendance.Create(
                await _attendanceRepository.GetNextIdAsync(cancellationToken),
                student.ClientId,
                request.WorkOffDatetime,
                course.Id,
                classesType.Id,
                teacher.Id,
                group?.Id,
                0,
                null,
                (int)AttendanceTypeEnum.WorkingOff,
                null,
                null,
                null,
                null);

            await _attendanceRepository.AddAsync(attendance, cancellationToken);

            var previousSubscription = await _studentSubscriptionRepository.
                GetByStudentCourseAndDateAsync(student.Id,
                course.Id,
                DateOnly.FromDateTime(request.OldDatetime),
                cancellationToken);
            if (previousSubscription == null)
                throw new ArgumentNullException();

            var newDate = DateOnly.FromDateTime(attendance.Datetime);
            if (newDate > previousSubscription.SubscriptionFinishDate)
            {
                var nextSubscription = await _studentSubscriptionRepository.
                    GetByStudentCourseAndDateAsync(student.Id,
                    course.Id,
                    newDate,
                    cancellationToken);

                if (nextSubscription != null && group != null)
                {
                    var nextAttendance = await _attendanceRepository
                        .GetNextClientGroupAsync(group.Id, student.ClientId, newDate);
                    if (nextAttendance != null)
                        await _studentSubscriptionRepository
                            .UpdateStartDateAsync(nextSubscription.Id, 
                            DateOnly.FromDateTime(nextAttendance.Datetime), cancellationToken);
                }

                await _studentSubscriptionRepository.UpdateFinishDateAsync(previousSubscription.Id, newDate);
            }

            if (group != null && !group.GroupDates.Any(gd => gd.Datetime.Date == request.WorkOffDatetime.Date))
            {
                var newGroupDate = GroupDate.Create(
                    await _groupDateRepository.GetNextIdAsync(),
                    group.Id,
                    request.WorkOffDatetime);
                await _groupDateRepository.AddAsync(newGroupDate, cancellationToken);
            }
        }
    }
}
