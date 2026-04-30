using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Commands;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.CourseOperations.Handlers.Commands
{
    public class RemoveCourseCommandHandler : IRequestHandler<RemoveCourseCommand>
    {
        private readonly ICourseRepository _courseRepository;

        public RemoveCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task Handle(RemoveCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (course == null)
                throw new ArgumentException("Данного курса не существует");

            await _courseRepository.RemoveAsync(course, cancellationToken);
        }
    }
}
