using System;
using System.Linq;
using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Commands;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Enums;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Commands
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupDateRepository _groupDateRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGroupStatusRepository _groupStatusRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public UpdateGroupCommandHandler(IGroupRepository groupRepository,
                                         IGroupDateRepository groupDateRepository,
                                         IStudentSubscriptionRepository studentSubscriptionRepository,
                                         ICourseRepository courseRepository,
                                         ITeacherRepository teacherRepository,
                                         IGroupStatusRepository groupStatusRepository,
                                         IStudentRepository studentRepository,
                                         IClientRepository clientRepository,
                                         IAttendanceRepository attendanceRepository)
        {
            _groupRepository = groupRepository;
            _groupDateRepository = groupDateRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _groupStatusRepository = groupStatusRepository;
            _studentRepository = studentRepository;
            _clientRepository = clientRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.Id, cancellationToken);
            if (group == null) throw new KeyNotFoundException("Группа не найдена");

            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course == null) throw new ArgumentNullException("Такого курса не существует");

            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
            if (teacher == null) throw new ArgumentNullException("Такого педагога не существует");

            var status = await _groupStatusRepository.GetByIdAsync(request.GroupStatusId, cancellationToken);
            if (status == null) throw new ArgumentNullException("Такого статуса группы не существует");


            group.Name = request.Name;
            group.CourseId = course.Id;
            group.TeacherId = teacher.Id;
            group.GroupStatusId = status.Id;
            group.WeekDays = request.WeekDays;
            group.Time = request.Time;
            await _groupRepository.UpdateAsync(group, cancellationToken);

            var updatedStudents = await SyncStudentsAsync(group, request.Students, request.Dates.Max(), cancellationToken);

            await SyncGroupDatesAsync(group, request.Dates, updatedStudents, cancellationToken);
        }

        private async Task SyncGroupDatesAsync(Group group, List<DateTime> newDates, List<Student> currentStudents, CancellationToken cancellationToken)
        {
            var existingDates = group.GroupDates.ToList();
            var incomingDateTimes = newDates.Select(d => new DateTime(DateOnly.FromDateTime(d), group.Time)).ToList();

            var datesToRemove = existingDates.Where(ed => !incomingDateTimes.Contains(ed.Datetime)).ToList();

            if (datesToRemove.Any())
            {
                var now = DateTime.Now;

                // Фильтруем только те, которые в прошлом, чтобы зря не гонять запросы
                var pastDatesToRemove = datesToRemove.Where(d => d.Datetime < now).Select(d => d.Datetime).ToList();

                if (pastDatesToRemove.Any())
                {
                    List<DateTime> lockedDates = await _attendanceRepository.GetDatesWithCompletedAttendanceAsync(group.Id, pastDatesToRemove, cancellationToken);

                    if (lockedDates.Any())
                    {
                        throw new InvalidOperationException(
                            $"Невозможно сохранить изменения. Следующие прошедшие даты уже содержат отмеченные посещения: {string.Join(", ", lockedDates.Select(d => d.ToString("dd.MM.yyyy")))}");
                    }
                }
            }

            foreach (var date in datesToRemove)
            {
                await _attendanceRepository.RemoveByGroupDateAsync(group.Id, date.Datetime, cancellationToken);
                await _groupDateRepository.RemoveAsync(date, cancellationToken);
            }

            var datesToAdd = incomingDateTimes.Where(id => !existingDates.Any(ed => ed.Datetime == id)).ToList();
            int nextGroupDateId = await _groupDateRepository.GetNextIdAsync(cancellationToken);

            var finalGroupDates = existingDates.Where(ed => !datesToRemove.Contains(ed)).ToList();

            foreach (var newDateTime in datesToAdd)
            {
                var groupDate = GroupDate.Create(nextGroupDateId++, group.Id, newDateTime);
                await _groupDateRepository.AddAsync(groupDate, cancellationToken);
                finalGroupDates.Add(groupDate);
                await CreateAttendanceForNewDateAsync(group, newDateTime, cancellationToken);
            }
            group.GroupDates = finalGroupDates;
        }

        private async Task<List<Student>> SyncStudentsAsync(Group group, List<StudentDTO> incomingStudentsDto, DateTime maxDate, CancellationToken cancellationToken)
        {
            var existingStudents = group.Students.ToList();
            var incomingIds = incomingStudentsDto.Select(s => s.Id).ToList();

            var studentsToRemove = existingStudents.Where(es => !incomingIds.Contains(es.Id)).ToList();
            if (studentsToRemove.Any())
            {
                await _attendanceRepository.RemoveFutureAttendanceForStudentsAsync(group.Id, studentsToRemove.Select(s => s.ClientId).ToList(), cancellationToken);

                existingStudents.RemoveAll(s => studentsToRemove.Contains(s));
            }

            var studentsToAddDto = incomingStudentsDto.Where(dto => !existingStudents.Any(es => es.Id == dto.Id)).ToList();
            foreach (var studentDto in studentsToAddDto)
            {
                var student = await _studentRepository.GetByIdAsync(studentDto.Id, cancellationToken);
                if (student == null) continue;

                existingStudents.Add(student);

                var subscriptionDto = studentDto.StudentSubscriptions.FirstOrDefault(ss =>
                    ss.Course.Id == group.CourseId &&
                    (ss.ClassesType.Id == (int)ClassesTypeEnum.Group || ss.ClassesType.Id == (int)ClassesTypeEnum.Intensive) &&
                    ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active);

                if (subscriptionDto != null)
                {
                    await _studentSubscriptionRepository.UpdateFinishDateAsync(subscriptionDto.Id, DateOnly.FromDateTime(maxDate));
                    int clientStatus = group.GroupStatusId == (int)GroupStatusEnum.Active ? (int)ClientStatusEnum.Learning : (int)ClientStatusEnum.Enrolled;
                    await _clientRepository.UpdateStatusAsync(student.ClientId, clientStatus);
                }

                await CreateAttendanceForNewStudentAsync(group, student, cancellationToken);
            }

            await _groupRepository.SetStudentsAsync(group.Id, existingStudents, cancellationToken);
            return existingStudents;
        }

        private async Task CreateAttendanceForNewDateAsync(Group group, DateTime dateTime, CancellationToken cancellationToken)
        {
            int id = await _attendanceRepository.GetNextIdAsync(cancellationToken);
            foreach (var student in group.Students)
            {
                var attendance = Attendance.Create(
                id,
                    student.ClientId,
                    dateTime,
                    group.CourseId,
                    (int)ClassesTypeEnum.Group,
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

        private async Task CreateAttendanceForNewStudentAsync(Group group, Student student, CancellationToken cancellationToken)
        {
            int id = await _attendanceRepository.GetNextIdAsync(cancellationToken);
            foreach (var date in group.GroupDates)
            {
                var attendance = Attendance.Create(
                id,
                    student.ClientId,
                    date.Datetime,
                    group.CourseId,
                    (int)ClassesTypeEnum.Group,
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
