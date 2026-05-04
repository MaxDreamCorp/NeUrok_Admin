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

        [ObservableProperty]
        private ObservableCollection<GroupStatusDTO> _statuses = new();

        [ObservableProperty]
        private List<string> _hours = Enumerable.Range(7, 23).Select(i => i.ToString("D2")).ToList();

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
            }

            OperationType = operationType;
        }
    }
}
