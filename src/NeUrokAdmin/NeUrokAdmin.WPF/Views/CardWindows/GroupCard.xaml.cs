using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
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
            ViewModel.Statuses = new(statuses);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

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
                Order().Select(d => d.ToString("ddd")).Distinct());
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
            var allStudents = await _mediator.Send(new GetAllStudentsQuery());

            var vm = new StudentsSelectorViewModel(allStudents, ViewModel.Students.ToList());
            var selectorWindow = _navigationService.GetWindow<StudentsSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.StudentsSelected += SelectorWindow_StudentsSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_StudentsSelected(object? sender, List<StudentDTO> e)
        {
            ViewModel.Students = new(e);
        }
    }
}
