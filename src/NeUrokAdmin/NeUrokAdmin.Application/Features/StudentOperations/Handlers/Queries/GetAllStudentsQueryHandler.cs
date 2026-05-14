using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentOperations.Handlers.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDTO>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository, IClientRepository clientRepository, IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _studentRepository = studentRepository;
            _clientRepository = clientRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task<List<StudentDTO>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync(cancellationToken);

            List<StudentDTO> result = new List<StudentDTO>();
            foreach (var student in students)
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
                            studentSubscription.Subscription.Id,
                            studentSubscription.Subscription.Name,
                            new(
                                studentSubscription.Subscription.ClassesType.Id,
                                studentSubscription.Subscription.ClassesType.Type),
                            studentSubscription.Subscription.Cost,
                            studentSubscription.Subscription.ClassesAmount),
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

                result.Add(new StudentDTO(
                    student.Id,
                    clientDto,
                    subscriptionsDtos.OrderBy(ss => ss.SubscriptionStatus.Id).ToList()));
            }

            return result;
        }
    }
}
