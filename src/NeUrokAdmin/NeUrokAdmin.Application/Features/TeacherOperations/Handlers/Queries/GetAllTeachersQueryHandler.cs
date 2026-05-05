using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.TeacherOperations.Handlers.Queries
{
    public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, List<TeacherDTO>>
    {
        private readonly ITeacherRepository _teacherRepository;

        public GetAllTeachersQueryHandler(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<List<TeacherDTO>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
        {
            var teachers = await _teacherRepository.GetAllAsync(cancellationToken);

            return teachers.Select(t => new TeacherDTO(
                t.Id,
                t.Fullname,
                t.IndividualLessonsShare,
                t.Notes)).ToList();
        }
    }
}
