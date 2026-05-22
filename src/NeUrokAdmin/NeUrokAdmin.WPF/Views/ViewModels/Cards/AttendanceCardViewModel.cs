using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Enums;

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
        private AttendanceStatusDTO? _status;

        [ObservableProperty]
        private AttendanceTypeDTO _type;

        [ObservableProperty]
        private decimal? _price;

        [ObservableProperty]
        private decimal? _teacherShare;

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
            _status = attendance.AttendanceStatus;
            _type = attendance.AttendanceType;
            _price = attendance.Price ??
                (studentSubscription.Cost / studentSubscription.ClassesAmount);
            if (attendance.TeacherShare.HasValue)
                _teacherShare = attendance.TeacherShare.Value;
            else
            {
                if (studentSubscription.ClassesType.Id == (int)ClassesTypeEnum.Individual)
                    _teacherShare = attendance.Teacher.IndividualLessonsShare;
                else
                {
                    if (attendance.Teacher.Id == 1)
                        _teacherShare = _price;
                    else
                        _teacherShare = _price * 0.6m;
                }
            }
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
                Status,
                Type,
                Price,
                TeacherShare);
        }
    }
}