using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Commands;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.ClientOperations.Handlers.Commands
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientStatusRepository _clientStatusRepository;
        private readonly ICourseRepository _courseRepository;

        public CreateClientCommandHandler(IClientRepository clientRepository, IClientStatusRepository clientStatusRepository, ICourseRepository courseRepository)
        {
            _clientRepository = clientRepository;
            _clientStatusRepository = clientStatusRepository;
            _courseRepository = courseRepository;
        }

        public async Task Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clientStatus = await _clientStatusRepository.GetByIdAsync(request.StatusId, cancellationToken);
            if (clientStatus == null)
                throw new ArgumentNullException("Данного статуса не существует");

            var wishedCourses = new List<Course>();

            if (request.WishedCoursesIds != null)
            {
                foreach (var courseId in request.WishedCoursesIds)
                {
                    var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
                    if (course == null)
                        throw new ArgumentNullException($"Данного курса с id {courseId} не существует");
                    wishedCourses.Add(course);
                }
            }

            var client = Client.Create(
                await _clientRepository.GetNextIdAsync(cancellationToken),
                request.ChildFullname,
                request.BirthDate,
                request.RegistrationDate,
                request.Grade,
                request.StatusId,
                request.ParentName,
                request.Phone,
                wishedCourses,
                request.AdditionalPhones,
                request.Notes);

            await _clientRepository.AddAsync(client, cancellationToken);
        }
    }
}
