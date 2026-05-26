namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentAttendancesDTO(
        StudentDTO Student,
        GroupDTO Group,
        List<AttendanceDTO> Attendances);
}

