using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<GroupDTO>>
    {
        private readonly IGroupRepository _groupRepository;

        public GetAllGroupsQueryHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<List<GroupDTO>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllAsync(cancellationToken);

            List<GroupDTO> result = new();

            result.AddRange(groups.Select(g => new GroupDTO(
                g.Id,
                g.Name,
                new(
                    g.Course.Id,
                    g.Course.Name),
                new(
                    g.Teacher.Id,
                    g.Teacher.Fullname,
                    g.Teacher.IndividualLessonsShare,
                    g.Teacher.Notes),
                new(
                    g.GroupStatus.Id,
                    g.GroupStatus.Status),
                g.WeekDays,
                g.Time)));

            return result;
        }
    }
}
