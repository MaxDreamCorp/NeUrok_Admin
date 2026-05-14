using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.ClientOperations.Handlers.Queries
{
    public class GetClientsByFilterQueryHandler : IRequestHandler<GetClientsByFilterQuery, List<ClientDTO>>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsByFilterQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<ClientDTO>> Handle(GetClientsByFilterQuery request, CancellationToken cancellationToken)
        {
            var clients = await _clientRepository.SearchAsync(request.Request, cancellationToken);

            List<ClientDTO> result = new List<ClientDTO>();

            result.AddRange(clients.Select(c => new ClientDTO(
                c.Id,
                c.ChildFullname,
                c.BirthDate,
                c.RegistrationDate,
                c.Grade,
                new(
                    c.Status.Id,
                    c.Status.Status),
                c.ParentName,
                c.Phone,
                c.Courses != null ?
                    c.Courses.Select(cr => new CourseDTO(
                        cr.Id,
                        cr.Name)).ToList() :
                    null,
                c.Notes,
                c.AdditionalPhones)));

            return result;
        }
    }
}
