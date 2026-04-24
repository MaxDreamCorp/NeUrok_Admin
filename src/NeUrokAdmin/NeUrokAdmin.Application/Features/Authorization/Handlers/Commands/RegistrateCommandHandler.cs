using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.Authorization.Handlers.Commands
{
    public class RegistrateCommandHandler : IRequestHandler<RegistrateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;

        public RegistrateCommandHandler(IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task Handle(RegistrateCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
            if (existingUser != null)
                throw new Exception("Пользователь с таким логином уже существует");

            var hashedPair = _hasher.Hash(request.Password);

            var newUser = User.Create(
                await _userRepository.GetNextIdAsync(cancellationToken),
                request.Login,
                hashedPair.hash,
                hashedPair.salt);

            await _userRepository.AddAsync(newUser, cancellationToken);
        }
    }
}
