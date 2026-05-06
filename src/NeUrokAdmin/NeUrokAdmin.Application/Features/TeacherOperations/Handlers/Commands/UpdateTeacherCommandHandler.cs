using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Handlers.Commands
{
    public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand>
    {
        private readonly ITeacherRepository _teacherRepository;

        public UpdateTeacherCommandHandler(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = Teacher.Create(
                request.Id,
                request.Fullname,
                request.IndividualLessonsShare,
                request.Notes);

            await _teacherRepository.UpdateAsync(teacher, cancellationToken);
        }
    }
}
