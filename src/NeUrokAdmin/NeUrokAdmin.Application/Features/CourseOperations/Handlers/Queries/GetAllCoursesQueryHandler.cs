using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.CourseOperations.Handlers.Queries
{
    public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDTO>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetAllCoursesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseDTO>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _courseRepository.GetAllAsync(cancellationToken);

            List<CourseDTO> result = new();

            result.AddRange(courses.Select(c => new CourseDTO(
                    c.Id,
                    c.Name)));

            return result;
        }
    }
}
