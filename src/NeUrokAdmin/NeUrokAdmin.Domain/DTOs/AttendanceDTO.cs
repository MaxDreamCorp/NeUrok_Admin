using NeUrokAdmin.Domain.Enums;

namespace NeUrokAdmin.Domain.DTOs
{
    public record AttendanceDTO(
        int Id,
        int ClientId,
        DateTime Datetime,
        CourseDTO Course,
        ClassesTypeDTO ClassesType,
        TeacherDTO Teacher,
        int? GroupId,
        bool IsComplited,
        AttendanceStatusDTO? AttendanceStatus,
        AttendanceTypeDTO AttendanceType,
        decimal? Price,
        decimal? TeacherShare)
    {
        public string Mark
        {
            get
            {
                if (!IsComplited) return "";
                else if (IsComplited && AttendanceStatus != null)
                {
                    return AttendanceStatus.Id switch
                    {
                        (int)AttendanceStatusEnum.Present => "+",
                        (int)AttendanceStatusEnum.Absent => "-",
                        (int)AttendanceStatusEnum.Excused => "у",
                        _ => ""

                    };
                }
                return "";
            }
        }
    }
}
