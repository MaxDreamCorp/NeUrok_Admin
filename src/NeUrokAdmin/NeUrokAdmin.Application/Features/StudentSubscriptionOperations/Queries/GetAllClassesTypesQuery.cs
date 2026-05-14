using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries
{
    public record GetAllClassesTypesQuery() : IRequest<List<ClassesTypeDTO>>;
}
