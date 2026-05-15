using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<GroupDTO>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public GetAllGroupsQueryHandler(IGroupRepository groupRepository, IClientRepository clientRepository, IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _groupRepository = groupRepository;
            _clientRepository = clientRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task<List<GroupDTO>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllAsync(cancellationToken);

            List<GroupDTO> result = new();

            foreach (var g in groups)
            {
                List<StudentDTO> students = new List<StudentDTO>();
                foreach (var student in g.Students)
                {
                    var client = await _clientRepository.GetByIdAsync(student.ClientId, cancellationToken);
                    if (client == null)
                        throw new Exception("У ученика отсутствует сущность клиента");

                    List<StudentSubscriptionDTO> subscriptionsDtos = new List<StudentSubscriptionDTO>();
                    foreach (var sSubscription in student.StudentSubscriptions)
                    {
                        var studentSubscription = await _studentSubscriptionRepository.GetByIdAsync(sSubscription.Id, cancellationToken);
                        if (studentSubscription == null)
                            throw new Exception("У данного ученика нет данного активного абонемента");

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

                    students.Add(new StudentDTO(
                        student.Id,
                        clientDto,
                        subscriptionsDtos.OrderBy(ss => ss.SubscriptionStatus.Id).ToList()));
                }

                result.Add(new GroupDTO(
                g.Id,
                g.Name,
                new(
                    g.Course.Id,
                    g.Course.Name),
                new(
                    g.Teacher.Id,
                    g.Teacher.Fullname,
                    g.Teacher.IndividualLessonsShare,
                    g.Teacher.Notes),
                new(
                    g.GroupStatus.Id,
                    g.GroupStatus.Status),
                g.WeekDays,
                g.Time,
                g.GroupDates.Select(gd => gd.Datetime).ToList(),
                students));
            }


            return result;
        }
    }
}
