using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.CourseOperations.Handlers.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var existingCourse = await _courseRepository.GetByNameAsync(request.Name, cancellationToken);

            if (existingCourse != null)
                throw new ArgumentException("Курс с таким названием уже существует");

            var newCourse = Course.Create(
                await _courseRepository.GetNextIdAsync(cancellationToken),
                request.Name);

            await _courseRepository.AddAsync(newCourse, cancellationToken);
        }
    }
}
