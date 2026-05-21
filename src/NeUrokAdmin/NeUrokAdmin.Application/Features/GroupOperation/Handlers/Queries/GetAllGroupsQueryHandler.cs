using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<GroupDTO>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly GettingService _gettingService;

        public GetAllGroupsQueryHandler(IGroupRepository groupRepository, GettingService gettingService)
        {
            _groupRepository = groupRepository;
            _gettingService = gettingService;
        }

        public async Task<List<GroupDTO>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllAsync(cancellationToken);

            List<GroupDTO> result = new();

            foreach (var g in groups)
                result.Add(await _gettingService.GetGroupDTOFromGroupAsync(g, cancellationToken));

            return result;
        }
    }
}
