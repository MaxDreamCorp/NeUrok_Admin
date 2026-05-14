using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Handlers.Commands
{
    public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand>
    {
        private readonly ITeacherRepository _teacherRepository;

        public CreateTeacherCommandHandler(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var existingTeacher = await _teacherRepository.GetByFullnameAsync(request.Fullname, cancellationToken);
            if (existingTeacher != null)
                throw new Exception("Учитель с таким ФИО уже существует");

            var teacher = Teacher.Create(
                await _teacherRepository.GetNextIdAsync(cancellationToken),
                request.Fullname,
                request.IndividualLessonsShare,
                request.Notes);

            await _teacherRepository.AddAsync(teacher, cancellationToken);
        }
    }
}
