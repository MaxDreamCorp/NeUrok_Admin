using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentOperations.Handlers.Queries
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDTO?>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly GettingService _gettingService;

        public GetStudentByIdQueryHandler(IStudentRepository studentRepository, GettingService gettingService)
        {
            _studentRepository = studentRepository;
            _gettingService = gettingService;
        }

        public async Task<StudentDTO?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (student == null)
                return null;

            return await _gettingService.GetStudentDTOFromStudentAsync(student, cancellationToken);
        }
    }
}
