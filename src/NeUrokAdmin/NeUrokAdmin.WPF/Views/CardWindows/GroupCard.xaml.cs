using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
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

        public GroupCardViewModel ViewModel { get; set; } = null!;

        public GroupCard(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
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

        private void SelectTeacherBtn_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
