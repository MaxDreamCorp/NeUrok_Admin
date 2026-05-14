using MediatR;

namespace NeUrokAdmin.Application.Features.ClientOperations.Commands
{
    public record CreateClientCommand(string ChildFullname,
        DateOnly? BirthDate,
        DateOnly RegistrationDate,
        int? Grade,
        int StatusId,
        string ParentName,
        string Phone,
        List<int>? WishedCoursesIds,
        string? Notes,
        string? AdditionalPhones) : IRequest;
}
