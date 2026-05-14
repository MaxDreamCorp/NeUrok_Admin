using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Commands;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Handlers.Commands
{
    public class RemoveTeacherCommandHandler : IRequestHandler<RemoveTeacherCommand>
    {
        private readonly ITeacherRepository _teacherRepository;

        public RemoveTeacherCommandHandler(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task Handle(RemoveTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetByIdAsync(request.Id, cancellationToken);
            if (teacher == null)
                throw new ArgumentNullException("Данного преподавателя не существует");

            await _teacherRepository.RemoveAsync(teacher, cancellationToken);
        }
    }
}
