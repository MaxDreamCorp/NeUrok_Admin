using MediatR;

namespace NeUrokAdmin.Application.Features.Authorization.Commands
{
    public record RegistrateCommand(string Login, string Password) : IRequest;
}
