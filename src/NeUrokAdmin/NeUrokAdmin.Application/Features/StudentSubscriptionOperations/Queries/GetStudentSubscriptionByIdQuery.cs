using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries
{
    public record GetStudentSubscriptionByIdQuery(int Id) : IRequest<StudentSubscriptionDTO?>;
}
