using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentSubscriptionCard.xaml
    /// </summary>
    public partial class StudentSubscriptionCard : Window
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public event EventHandler<StudentSubscriptionDTO>? StudentSubscriptionCreated;
        public event EventHandler<StudentSubscriptionDTO>? StudentSubscriptionEdited;

        public StudentSubscriptionCardViewModel ViewModel { get; set; } = null!;

        public StudentSubscriptionCard(IMediator mediator, NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            ViewModel.ClassesTypesDTO = await _mediator.Send(new GetAllClassesTypesQuery());
            ViewModel.SubscriptionStatusesDTO = await _mediator.Send(new GetAllSubscriptionStatusesQuery());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields())
                return;

            try
            {
                var dto = ViewModel.GetStudentSubscriptionDTO();
                if (ViewModel.OperationType == Enums.OperationType.Create)
                    StudentSubscriptionCreated?.Invoke(this, dto);
                else if (ViewModel.OperationType == Enums.OperationType.Edit)
                    StudentSubscriptionEdited?.Invoke(this, dto);
                Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
            }
        }

        private bool CheckFields()
        {
            if (string.IsNullOrEmpty(ViewModel.StudentFullname))
            {
                _dialogService.ShowWarning("Ученик не может быть пустым");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.ClassesType))
            {
                _dialogService.ShowWarning("Тип занятий не может быть пустым");
                return false;
            }
            if (ViewModel.Cost == null)
            {
                _dialogService.ShowWarning("Стоимость не может быть пустой");
                return false;
            }
            if (ViewModel.Course == null)
            {
                _dialogService.ShowWarning("Курс не может быть пустым");
                return false;
            }
            if (ViewModel.ClassesAmount == null)
            {
                _dialogService.ShowWarning("Кол-во занятий не может быть пустым");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.SubscriptionStatusStr))
            {
                _dialogService.ShowWarning("Статус абонемента не может быть пустым");
                return false;
            }
            if (ViewModel.StartDate == null)
            {
                _dialogService.ShowWarning("Начало не может быть пустым");
                return false;
            }
            return true;
        }

    }
}
