using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentOperations.Commands
{
    public record CreateStudentCommand(
        int ClientId,
        List<StudentSubscriptionDTO> StudentSubscriptions) : IRequest;
}
