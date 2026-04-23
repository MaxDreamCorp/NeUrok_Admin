using MediatR;

namespace NeUrokAdmin.Application.Features.Authorization.Commands
{
    public record LoginCommand(string Login, string Password) : IRequest;
}
