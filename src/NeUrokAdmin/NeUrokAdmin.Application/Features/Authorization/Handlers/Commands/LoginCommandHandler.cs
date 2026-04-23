using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.Domain.Interfaces;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.Authorization.Handlers.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;

        public LoginCommandHandler(IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
            if (user == null)
                throw new Exception("Такого пользователя не существует");

            var isPasswordCorrect = _hasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordCorrect)
                throw new Exception("Неверный пароль");
        }
    }
}
