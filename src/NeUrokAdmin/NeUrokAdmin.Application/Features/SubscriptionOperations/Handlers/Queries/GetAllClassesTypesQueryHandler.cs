using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.SubscriptionOperations.Handlers.Queries
{
    public class GetAllClassesTypesQueryHandler : IRequestHandler<GetAllClassesTypesQuery, List<ClassesTypeDTO>>
    {
        private readonly IClassesTypeRepository _classesTypeRepository;

        public GetAllClassesTypesQueryHandler(IClassesTypeRepository classesTypeRepository)
        {
            _classesTypeRepository = classesTypeRepository;
        }

        public async Task<List<ClassesTypeDTO>> Handle(GetAllClassesTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _classesTypeRepository.GetAllAsync(cancellationToken);
            return types.Select(t => new ClassesTypeDTO(t.Id, t.Type)).ToList();
        }
    }
}
