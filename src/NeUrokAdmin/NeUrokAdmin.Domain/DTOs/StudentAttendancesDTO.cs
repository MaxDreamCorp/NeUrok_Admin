namespace NeUrokAdmin.Domain.DTOs
{
    public record StudentAttendancesDTO(
        StudentDTO Student,
        StudentSubscriptionDTO StudentSubscription,
        GroupDTO? Group,
        List<AttendanceDTO> Attendances);
}

