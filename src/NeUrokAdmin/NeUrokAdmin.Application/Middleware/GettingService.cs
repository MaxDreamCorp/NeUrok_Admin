using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Middleware
{
    public class GettingService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public GettingService(IClientRepository clientRepository, IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _clientRepository = clientRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task<StudentDTO> GetStudentDTOFromStudentAsync(Student student, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdAsync(student.ClientId, cancellationToken);
            if (client == null)
                throw new Exception("У ученика отсутствует сущность клиента");

            List<StudentSubscriptionDTO> subscriptionsDtos = new List<StudentSubscriptionDTO>();
            var subscriptions = await _studentSubscriptionRepository.GetByStudentIdAsync(student.Id, cancellationToken);
            foreach (var studentSubscription in subscriptions)
            {
                subscriptionsDtos.Add(new(
                    studentSubscription.Id,
                    student.Id,
                    new(
                        studentSubscription.ClassesType.Id,
                        studentSubscription.ClassesType.Type),
                    studentSubscription.Cost,
                    studentSubscription.ClassesAmount,
                    studentSubscription.IsPaid == 1,
                    new(
                        studentSubscription.Course.Id,
                        studentSubscription.Course.Name),
                    new(
                        studentSubscription.SubscriptlonStatus.Id,
                        studentSubscription.SubscriptlonStatus.Status),
                    studentSubscription.SubscriptionStartDate,
                    studentSubscription.SubscriptionFinishDate));
            }

            var clientDto = new ClientDTO(
            client.Id,
            client.ChildFullname,
            client.BirthDate,
            client.RegistrationDate,
            client.Grade,
            new(
                client.Status.Id,
                client.Status.Status),
            client.ParentName,
            client.Phone,
            client.Courses != null ?
                client.Courses.Select(cr => new CourseDTO(
                    cr.Id,
                    cr.Name)).ToList() :
                null,
            client.Notes,
            client.AdditionalPhones);

            return new StudentDTO(
                student.Id,
                clientDto,
                subscriptionsDtos.OrderBy(ss => ss.SubscriptionStatus.Id).ToList());
        }

        public async Task<GroupDTO> GetGroupDTOFromGroupAsync(Group group, CancellationToken cancellationToken = default)
        {
            List<StudentDTO> students = new List<StudentDTO>();
            foreach (var student in group.Students)
                students.Add(await GetStudentDTOFromStudentAsync(student, cancellationToken));

            return new GroupDTO(
             group.Id,
             group.Name,
             new(
                 group.Course.Id,
                 group.Course.Name),
             new(
                 group.Teacher.Id,
                 group.Teacher.Fullname,
                 group.Teacher.IndividualLessonsShare,
                 group.Teacher.Notes),
             new(
                 group.GroupStatus.Id,
                 group.GroupStatus.Status),
             group.WeekDays,
             group.Time,
             group.GroupDates.Select(gd => gd.Datetime).ToList(),
             students);
        }
    }
}
