using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentOperations.Handlers.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;
        private readonly IClientRepository _clientRepository;

        public CreateStudentCommandHandler(IStudentRepository studentRepository, IStudentSubscriptionRepository studentSubscriptionRepository, IClientRepository clientRepository)
        {
            _studentRepository = studentRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
            _clientRepository = clientRepository;
        }

        public async Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
                throw new Exception("Данного клиента не существует");

            var student = Student.Create(
                await _studentRepository.GetNextIdAsync(cancellationToken),
                client.Id);
            await _studentRepository.AddAsync(student, cancellationToken);

            foreach (var subscription in request.StudentSubscriptions)
            {
                var studentSubscription = StudentSubscription.Create(
                    await _studentSubscriptionRepository.GetNextIdAsync(cancellationToken),
                    student.Id,
                    subscription.Subscription.Id,
                    (sbyte)(subscription.IsPaid ? 1 : 0),
                    subscription.Course.Id,
                    subscription.SubscriptionStatus.Id,
                    subscription.StartDate,
                    subscription.FinishDate);

                await _studentSubscriptionRepository.AddAsync(studentSubscription, cancellationToken);
            }
        }
    }
}
