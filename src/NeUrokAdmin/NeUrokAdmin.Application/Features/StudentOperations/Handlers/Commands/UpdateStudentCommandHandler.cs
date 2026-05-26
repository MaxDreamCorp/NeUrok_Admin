using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentOperations.Handlers.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository, IStudentSubscriptionRepository studentSubscriptionRepository)
        {
            _studentRepository = studentRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
        }

        public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (student == null)
                throw new Exception("Данного ученика не существует");

            var existingSubscription = student.StudentSubscriptions;

            var requestIds = request.StudentSubscriptions.Select(ss => ss.Id).ToList();
            var toDelete = existingSubscription.Where(ss => !requestIds.Contains(ss.Id)).ToList();
            foreach (var sub in toDelete)
                await _studentSubscriptionRepository.RemoveAsync(sub, cancellationToken);

            foreach (var dto in request.StudentSubscriptions)
            {
                if (dto.Id == 0)
                {
                    var studentSubscription = StudentSubscription.Create(
                        await _studentSubscriptionRepository.GetNextIdAsync(cancellationToken),
                        student.Id,
                        dto.ClassesType.Id,
                        dto.Cost,
                        dto.ClassesAmount,
                        (sbyte)(dto.IsPaid ? 1 : 0),
                        dto.Course.Id,
                        dto.SubscriptionStatus.Id,
                        dto.StartDate,
                        dto.FinishDate);
                    await _studentSubscriptionRepository.AddAsync(studentSubscription, cancellationToken);
                }
                else
                {
                    if (existingSubscription.Any(ss => ss.Id == dto.Id))
                    {
                        var studentSubscription = StudentSubscription.Create(
                            dto.Id,
                            student.Id,
                            dto.ClassesType.Id,
                            dto.Cost,
                            dto.ClassesAmount,
                            (sbyte)(dto.IsPaid ? 1 : 0),
                            dto.Course.Id,
                            dto.SubscriptionStatus.Id,
                            dto.StartDate,
                            dto.FinishDate);
                        await _studentSubscriptionRepository.UpdateAsync(studentSubscription, cancellationToken);
                    }
                }
            }
        }
    }
}
