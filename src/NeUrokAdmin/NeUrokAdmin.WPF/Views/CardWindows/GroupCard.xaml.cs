using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Application.Features.GroupOperation.Commands;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Enums;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для GroupCard.xaml
    /// </summary>
    public partial class GroupCard : Window
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public GroupCardViewModel ViewModel { get; set; } = null!;

        public GroupCard(IMediator mediator, NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var statuses = await _mediator.Send(new GetAllGroupStatusesQuery());
            ViewModel.GroupStatusDTOs = new(statuses);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            switch (ViewModel.OperationType)
            {
                case Enums.OperationType.Create:
                    result = await CreateGroup();
                    break;
                case Enums.OperationType.Read:
                    break;
                case Enums.OperationType.Edit:
                    //result = await UpdateGroup();
                    break;
                case Enums.OperationType.Filter:
                    result = true;
                    break;
                default:
                    break;
            }

            if (result)
            {
                DialogResult = true;
                Close();
            }
        }

        private async void SelectCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            var allCourses = await _mediator.Send(new GetAllCoursesQuery());

            var vm = new CoursesSelectorViewModel(allCourses, ViewModel.Course);
            var selectorWindow = _navigationService.GetWindow<CoursesSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.CoursesSelected += SelectorWindow_CoursesSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_CoursesSelected(object? sender, List<CourseDTO> e)
        {
            if (e.Count > 0)
                ViewModel.Course = e[0];
        }

        private async void SelectTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            var allTeachers = await _mediator.Send(new GetAllTeachersQuery());

            var vm = new TeacherSelectorViewModel(allTeachers, ViewModel.Teacher);
            var selectorWindow = _navigationService.GetWindow<TeachersSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.TeachersSelected += SelectorWindow_TeachersSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_TeachersSelected(object? sender, List<TeacherDTO> e)
        {
            if (e.Count > 0)
                ViewModel.Teacher = e[0];
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                ViewModel.SelectedDates = calendar.SelectedDates
                    .Order().ToList();
                ViewModel.ClassesDates = string.Join(", ", ViewModel.SelectedDates
                    .Order().Select(d =>
                    $"Занятие {ViewModel.SelectedDates.IndexOf(d) + 1}: {d.ToShortDateString()}"));
            }
        }

        private void AcceptDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.WeekDays = string.Join(", ", ViewModel.SelectedDates.
                OrderBy(d => d.DayOfWeek).Select(d => d.ToString("ddd")).Distinct());
        }

        private void RemoveStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDg.SelectedItem is StudentDTO studentDTO)
            {
                if (!_dialogService.AskQuetion("Вы уверены, что хотите удалить запись?"))
                    return;

                ViewModel.Students.Remove(studentDTO);
            }
        }

        private async void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Course == null)
            {
                _dialogService.ShowWarning("Сначала выберите курс");
                return;
            }
            var allStudents = await _mediator.Send(new GetAllStudentsQuery());

            var vm = new StudentsSelectorViewModel(allStudents, ViewModel.Students.ToList());
            var selectorWindow = _navigationService.GetWindow<StudentsSelectorWindow>();
            if (ViewModel.Course != null)
                vm.QuickSearchText = ViewModel.Course.Name;
            selectorWindow.ViewModel = vm;
            selectorWindow.StudentsSelected += SelectorWindow_StudentsSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_StudentsSelected(object? sender, List<StudentDTO> e)
        {
            if (ViewModel.Course == null)
                return;

            List<StudentDTO> result = new List<StudentDTO>();
            foreach (var studentDTO in e)
            {
                if (!studentDTO.StudentSubscriptions.Any(ss =>
                    ss.Course.Id == ViewModel.Course.Id &&
                    (ss.ClassesType.Id == (int)ClassesTypeEnum.Group || ss.ClassesType.Id == (int)ClassesTypeEnum.Intensive) &&
                    ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active))
                {
                    _dialogService.ShowWarning($"Ни один из групповых абонементов ученика {studentDTO.Client.ChildFullname} не выписан на \"{ViewModel.Course.Name}\"");
                    continue;
                }

                result.Add(studentDTO);
            }

            ViewModel.Students = new(result);
        }

        private bool CheckFields()
        {
            if (string.IsNullOrEmpty(ViewModel.Name))
            {
                _dialogService.ShowWarning("Название не может быть пустым");
                return false;
            }
            if (ViewModel.Course == null)
            {
                _dialogService.ShowWarning("Курс не выбран");
                return false;
            }
            if (ViewModel.Teacher == null)
            {
                _dialogService.ShowWarning("Педагог не выбран");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.SelectedStatus))
            {
                _dialogService.ShowWarning("Статус не может быть пустым");
                return false;
            }
            if (!ViewModel.ClassesDates.Any())
            {
                _dialogService.ShowWarning("Даты занятий не выбраны");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.WeekDays))
            {
                _dialogService.ShowWarning("Дни недели не может быть пустыми");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.TimeHours) || string.IsNullOrEmpty(ViewModel.TimeMinutes))
            {
                _dialogService.ShowWarning("Время не может быть пустым");
                return false;
            }
            if (!ViewModel.Students.Any())
            {
                _dialogService.ShowWarning("Ученики занятий не выбраны");
                return false;
            }
            return true;
        }

        private async Task<bool> CreateGroup()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать новую группу?"))
                return false;

            if (!CheckFields())
                return false;

            try
            {
                GroupDTO dto = ViewModel.GetGroupDTO();

                var cmd = new CreateGroupCommand(
                    dto.Name,
                    dto.Course.Id,
                    dto.Teacher.Id,
                    dto.GroupStatus.Id,
                    dto.WeekDays,
                    dto.Time,
                    dto.Dates,
                    dto.Students);

                var groupId = await _mediator.Send(cmd);
                await _mediator.Send(new CreateAttendancesForGroupCommand(groupId, (int)ClassesTypeEnum.Group)); // TODO: select classes type
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }
    }
}
