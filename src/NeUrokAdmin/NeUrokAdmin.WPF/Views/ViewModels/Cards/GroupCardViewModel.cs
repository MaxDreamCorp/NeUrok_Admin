using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class GroupCardViewModel : ObservableObject
    {
        public OperationType OperationType
        {
            get => _operationType;
            set
            {
                SetProperty(ref _operationType, value);

                switch (value)
                {
                    case OperationType.Create:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Создание группы";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Группа \"{Name}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Группа \"{Name}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр групп";
                        break;
                    default:
                        break;
                }
            }
        }

        private OperationType _operationType;

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private bool _isFilter;

        [ObservableProperty]
        private string _headerText = null!;

        public List<GroupStatusDTO> GroupStatusDTOs
        {
            get => _groupStatusDTOs;
            set
            {
                _groupStatusDTOs = value;
                Statuses = new(value.Select(s => s.Status).ToList());
            }
        }

        private List<GroupStatusDTO> _groupStatusDTOs = new List<GroupStatusDTO>();

        [ObservableProperty]
        private List<string> _statuses = new();

        [ObservableProperty]
        private List<string> _hours = Enumerable.Range(7, 17).Select(i => i.ToString("D2")).ToList();

        [ObservableProperty]
        private List<string> _minutes = Enumerable.Range(0, 59).Where(i => i % 5 == 0).Select(i => i.ToString("D2")).ToList();

        public List<DateTime> SelectedDates { get; set; } = new();


        [ObservableProperty]
        private int? _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private CourseDTO? _course;

        [ObservableProperty]
        private TeacherDTO? _teacher;

        [ObservableProperty]
        private string _selectedStatus = string.Empty;

        [ObservableProperty]
        private string _classesDates = string.Empty;

        [ObservableProperty]
        private string _weekDays = string.Empty;

        [ObservableProperty]
        private string _timeHours = string.Empty;

        [ObservableProperty]
        private string _timeMinutes = string.Empty;

        [ObservableProperty]
        private TimeOnly? _time;

        [ObservableProperty]
        private ObservableCollection<StudentDTO> _students = new();


        public GroupCardViewModel(OperationType operationType, GroupDTO? groupDTO = null)
        {
            if (groupDTO != null)
            {
                _id = groupDTO.Id;
                _name = groupDTO.Name;
                _course = groupDTO.Course;
                _teacher = groupDTO.Teacher;
                _selectedStatus = groupDTO.GroupStatus.Status;
                _weekDays = groupDTO.WeekDays;
                _time = groupDTO.Time;
                _students = new(groupDTO.Students);
            }

            OperationType = operationType;
        }

        public GroupDTO GetGroupDTO()
        {
            if (string.IsNullOrEmpty(TimeHours) || string.IsNullOrEmpty(TimeMinutes))
                throw new ArgumentException(nameof(Time));

            Time = new(int.Parse(TimeHours), int.Parse(TimeMinutes));

            return new GroupDTO(
                Id: Id ?? 0,
                Name: Name,
                Course: Course ?? throw new InvalidOperationException("Не выбран курс"),
                Teacher: Teacher ?? throw new InvalidOperationException("Не выбран преподаватель"),
                GroupStatus: GroupStatusDTOs.FirstOrDefault(s => s.Status == SelectedStatus)
                    ?? throw new InvalidOperationException("Не выбран статус группы"),
                WeekDays: WeekDays,
                Time: Time ?? throw new InvalidOperationException("Не выбрано время"),
                Dates: SelectedDates,
                Students: Students.ToList()
    );

        }
    }
}
