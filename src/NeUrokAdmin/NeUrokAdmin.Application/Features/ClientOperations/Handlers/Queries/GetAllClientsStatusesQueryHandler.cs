using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.ClientOperations.Handlers.Queries
{
    public class GetAllClientsStatusesQueryHandler : IRequestHandler<GetAllClientsStatusesQuery, List<ClientStatusDTO>>
    {
        private readonly IClientStatusRepository _clientStatusRepository;

        public GetAllClientsStatusesQueryHandler(IClientStatusRepository clientStatusRepository)
        {
            _clientStatusRepository = clientStatusRepository;
        }

        public async Task<List<ClientStatusDTO>> Handle(GetAllClientsStatusesQuery request, CancellationToken cancellationToken)
        {
            var clientStatuses = await _clientStatusRepository.GetAllAsync(cancellationToken);

            return clientStatuses.Select(cs => new ClientStatusDTO(
                cs.Id,
                cs.Status))
                .ToList();
        }
    }
}
