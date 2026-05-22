using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Application.Middleware;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Application.Features.GroupOperation.Handlers.Queries
{
    public class GetGroupStudentsAttendancesCommandHandler : IRequestHandler<GetGroupStudentsAttendancesCommand, List<StudentAttendancesDTO>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly GettingService _gettingService;

        public GetGroupStudentsAttendancesCommandHandler(IGroupRepository groupRepository, IAttendanceRepository attendanceRepository, IClientRepository clientRepository, GettingService gettingService)
        {
            _groupRepository = groupRepository;
            _attendanceRepository = attendanceRepository;
            _clientRepository = clientRepository;
            _gettingService = gettingService;
        }

        public async Task<List<StudentAttendancesDTO>> Handle(GetGroupStudentsAttendancesCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null) throw new KeyNotFoundException("Группа не найдена");

            var groupDto = await _gettingService.GetGroupDTOFromGroupAsync(group, cancellationToken);
            List<StudentAttendancesDTO> result = new();
            foreach (var student in group.Students)
            {
                var studentAttendance = await _attendanceRepository.GetByGroupAndClientIdAsync(group.Id, student.ClientId, cancellationToken);

                var studentDto = await _gettingService.GetStudentDTOFromStudentAsync(student, cancellationToken);

                result.Add(new(
                    studentDto,
                    groupDto,
                    studentAttendance.Select(sa => new AttendanceDTO(
                        sa.Id,
                        student.ClientId,
                        sa.Datetime,
                        new(
                            sa.Course.Id,
                            sa.Course.Name),
                        new(
                            sa.ClassType.Id,
                            sa.ClassType.Type),
                        new(
                            sa.Teacher.Id,
                            sa.Teacher.Fullname,
                            sa.Teacher.IndividualLessonsShare,
                            sa.Teacher.Notes),
                        group.Id,
                        sa.IsCompleted == 1,
                        sa.AttendanceStatus != null ? new(
                            sa.AttendanceStatus.Id,
                            sa.AttendanceStatus.Status) :
                            null,
                        new(
                            sa.AttendanceType.Id,
                            sa.AttendanceType.Type),
                        sa.Price,
                        sa.TeacherShare))
                    .ToList()));
            }
            return result;
        }
    }
}
