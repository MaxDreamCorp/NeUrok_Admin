using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Commands
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IClassesTypeRepository _classesTypeRepository;

        public CreateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IClassesTypeRepository classesTypeRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _classesTypeRepository = classesTypeRepository;
        }

        public async Task Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var classesType = await _classesTypeRepository.GetByIdAsync(request.ClassesTypeId, cancellationToken);
            if (classesType == null)
                throw new Exception("Данного типа занятий не существует");

            var newSubscription = Subscription.Create(
                await _subscriptionRepository.GetNextIdAsync(cancellationToken),
                request.Name,
                request.ClassesTypeId,
                request.Cost,
                request.ClassesAmount);

            await _subscriptionRepository.AddAsync(newSubscription, cancellationToken);
        }
    }
}
