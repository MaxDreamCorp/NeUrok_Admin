using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Commands
{
    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IClassesTypeRepository _classesTypeRepository;

        public UpdateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IClassesTypeRepository classesTypeRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _classesTypeRepository = classesTypeRepository;
        }

        public async Task Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var classesType = await _classesTypeRepository.GetByIdAsync(request.ClassesTypeId, cancellationToken);
            if (classesType == null)
                throw new Exception("Данного типа занятий не существует");

            var subscription = Subscription.Create(
                request.Id,
                request.Name,
                request.ClassesTypeId,
                request.Cost,
                request.ClassesAmount);

            await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        }
    }
}
