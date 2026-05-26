using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Enums;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Commands
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, int>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupDateRepository _groupDateRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGroupStatusRepository _groupStatusRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClientRepository _clientRepository;

        public CreateGroupCommandHandler(IGroupRepository groupRepository,
                                         IGroupDateRepository groupDateRepository,
                                         IStudentSubscriptionRepository studentSubscriptionRepository,
                                         ICourseRepository courseRepository,
                                         ITeacherRepository teacherRepository,
                                         IGroupStatusRepository groupStatusRepository,
                                         IStudentRepository studentRepository,
                                         IClientRepository clientRepository)
        {
            _groupRepository = groupRepository;
            _groupDateRepository = groupDateRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _groupStatusRepository = groupStatusRepository;
            _studentRepository = studentRepository;
            _clientRepository = clientRepository;
        }

        public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course == null)
                throw new ArgumentNullException("Такого курса не существует");

            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
            if (teacher == null)
                throw new ArgumentNullException("Такого педагога не существует");

            var status = await _groupStatusRepository.GetByIdAsync(request.GroupStatusId, cancellationToken);
            if (status == null)
                throw new ArgumentNullException("Такого статуса группы не существует");


            var group = Group.Create(
                0,
                request.Name,
                course.Id,
                teacher.Id,
                status.Id,
                request.WeekDays,
                request.Time);
            await _groupRepository.AddAsync(group, cancellationToken);

            int groupDateId = await _groupDateRepository.GetNextIdAsync(cancellationToken);
            foreach (var date in request.Dates)
            {
                var dt = new DateTime(DateOnly.FromDateTime(date), group.Time);
                var groupDate = GroupDate.Create(
                    groupDateId,
                    group.Id,
                    dt);
                await _groupDateRepository.AddAsync(groupDate, cancellationToken);
                groupDateId++;
            }

            List<Student> students = new List<Student>();
            foreach (var studentDto in request.Students)
            {
                var student = await _studentRepository.GetByIdAsync(studentDto.Id, cancellationToken);
                if (student == null)
                    continue;

                students.Add(student);

                var subscriptionDto = studentDto.StudentSubscriptions.FirstOrDefault(ss =>
                    ss.Course.Id == group.CourseId &&
                    (ss.ClassesType.Id == (int)ClassesTypeEnum.Group || ss.ClassesType.Id == (int)ClassesTypeEnum.Intensive) &&
                    ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active);
                if (subscriptionDto == null)
                    continue;
                await _studentSubscriptionRepository.UpdateFinishDateAsync(subscriptionDto.Id, DateOnly.FromDateTime(request.Dates.Max()));
                if (group.GroupStatusId <= (int)GroupStatusEnum.Recruited)
                    await _clientRepository.UpdateStatusAsync(student.ClientId, (int)ClientStatusEnum.Enrolled);
                else if (group.GroupStatusId == (int)GroupStatusEnum.Active)
                    await _clientRepository.UpdateStatusAsync(student.ClientId, (int)ClientStatusEnum.Learning);
            }
            await _groupRepository.SetStudentsAsync(group.Id, students, cancellationToken);

            return group.Id;
        }
    }
}
