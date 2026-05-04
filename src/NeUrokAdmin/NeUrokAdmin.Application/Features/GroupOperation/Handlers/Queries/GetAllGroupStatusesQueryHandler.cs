using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetAllGroupStatusesQueryHandler : IRequestHandler<GetAllGroupStatusesQuery, List<GroupStatusDTO>>
    {
        private readonly IGroupStatusRepository _groupStatusRepository;

        public GetAllGroupStatusesQueryHandler(IGroupStatusRepository groupStatusRepository)
        {
            _groupStatusRepository = groupStatusRepository;
        }

        public async Task<List<GroupStatusDTO>> Handle(GetAllGroupStatusesQuery request, CancellationToken cancellationToken)
        {
            var groupStatuses = await _groupStatusRepository.GetAllAsync(cancellationToken);

            return groupStatuses.Select(gs => new GroupStatusDTO(
                gs.Id,
                gs.Status)).ToList();
        }
    }
}
