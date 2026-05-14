using MediatR;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.Application.Features.StudentOperations.Commands
{
    public record UpdateStudentCommand(
        int Id,
        List<StudentSubscriptionDTO> StudentSubscriptions) : IRequest;
}
