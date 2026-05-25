using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class AttendanceCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> _attendanceStatuses = new();

        public List<AttendanceStatusDTO> AttendanceStatusesDTO
        {
            get => _attendanceStatusesDTO;
            set
            {
                SetProperty(ref _attendanceStatusesDTO, value);
                AttendanceStatuses = value.Select(cs => cs.Status).ToList();
            }
        }
        private List<AttendanceStatusDTO> _attendanceStatusesDTO = new();

        public StudentSubscriptionDTO StudentSubscription { get; set; }

        [ObservableProperty]
        private bool _isGroup;

        private int _id;

        [ObservableProperty]
        private StudentDTO _student;

        [ObservableProperty]
        private GroupDTO? _group;

        [ObservableProperty]
        private DateTime _datetime;

        [ObservableProperty]
        private CourseDTO _course;

        [ObservableProperty]
        private ClassesTypeDTO _classesType;

        [ObservableProperty]
        private TeacherDTO _teacher;

        [ObservableProperty]
        private bool _isCompleted;

        [ObservableProperty]
        private string? _status;

        [ObservableProperty]
        private AttendanceTypeDTO _type;

        [ObservableProperty]
        private decimal? _price;

        [ObservableProperty]
        private decimal? _teacherShare;

        [ObservableProperty]
        private string? _absentCause;

        [ObservableProperty]
        private string? _notes;

        public AttendanceCardViewModel(GroupDTO? group, StudentDTO student, AttendanceDTO attendance, StudentSubscriptionDTO studentSubscription)
        {
            _student = student;
            _group = group;
            _isGroup = group != null;

            _id = attendance.Id;
            _datetime = attendance.Datetime;
            _course = attendance.Course;
            _classesType = attendance.ClassesType;
            _teacher = attendance.Teacher;
            _isCompleted = attendance.IsComplited;
            _status = attendance.AttendanceStatus?.Status ?? null;
            _type = attendance.AttendanceType;
            _absentCause = attendance.AbsentCause;
            _notes = attendance.Notes;
            _price = attendance.Price;
            _teacherShare = attendance.TeacherShare;
            StudentSubscription = studentSubscription;

        }

        public AttendanceDTO GetAttendanceDTO()
        {
            return new AttendanceDTO(
                _id,
                Student.Client.Id,
                Datetime,
                Course,
                ClassesType,
                Teacher,
                Group?.Id ?? null,
                IsCompleted,
                AttendanceStatusesDTO.FirstOrDefault(s => s.Status == Status),
                Type,
                Price,
                TeacherShare,
                string.IsNullOrEmpty(AbsentCause) ? null : AbsentCause,
                string.IsNullOrEmpty(Notes) ? null : Notes);
        }
    }
}