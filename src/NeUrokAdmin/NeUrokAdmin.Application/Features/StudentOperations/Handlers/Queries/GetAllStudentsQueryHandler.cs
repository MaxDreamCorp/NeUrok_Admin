using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.StudentOperations.Handlers.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDTO>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IStudentSubscriptionRepository _studentSubscriptionRepository;
        private readonly GettingService _gettingService;

        public GetAllStudentsQueryHandler(IStudentRepository studentRepository, IClientRepository clientRepository, IStudentSubscriptionRepository studentSubscriptionRepository, GettingService gettingService)
        {
            _studentRepository = studentRepository;
            _clientRepository = clientRepository;
            _studentSubscriptionRepository = studentSubscriptionRepository;
            _gettingService = gettingService;
        }

        public async Task<List<StudentDTO>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync(cancellationToken);

            List<StudentDTO> result = new List<StudentDTO>();
            foreach (var student in students)
            {
                var studentDto = await _gettingService.GetStudentDTOFromStudentAsync(student, cancellationToken);
                result.Add(studentDto);
            }

            return result;
        }
    }
}
