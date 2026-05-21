using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDTO>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly GettingService _gettingService;

        public GetGroupByIdQueryHandler(IGroupRepository groupRepository, GettingService gettingService)
        {
            _groupRepository = groupRepository;
            _gettingService = gettingService;
        }

        public async Task<GroupDTO> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null) throw new KeyNotFoundException("Группа не найдена");

            return await _gettingService.GetGroupDTOFromGroupAsync(group, cancellationToken);
        }
    }
}
