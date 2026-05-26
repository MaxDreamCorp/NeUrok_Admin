using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Commands;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.ClientOperations.Handlers.Commands
{
    public class UpdateGradesCommandHandler : IRequestHandler<UpdateGradesCommand>
    {
        private readonly IClientRepository _clientRepository;

        public UpdateGradesCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task Handle(UpdateGradesCommand request, CancellationToken cancellationToken)
        {
            await _clientRepository.UpdateGradesAsync(cancellationToken);
        }
    }
}
