using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Commands;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для CourseCard.xaml
    /// </summary>
    public partial class CourseCard : Window
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public CourseCardViewModel ViewModel { get; set; } = null!;

        public CourseCard(IMediator mediator, NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
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
                    result = await CreateCourse();
                    break;
                case Enums.OperationType.Read:
                    break;
                case Enums.OperationType.Edit:
                    result = await EditCourse();
                    break;
                case Enums.OperationType.Filter:
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

        private async Task<bool> CreateCourse()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать новый курс?"))
                return false;

            if (!CheckFields())
                return false;

            var cmd = new CreateCourseCommand(ViewModel.Name);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private async Task<bool> EditCourse()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите изменить данные курса?"))
                return false;

            if (!CheckFields())
                return false;

            var cmd = new UpdateCourseCommand(
                ViewModel.Id,
                ViewModel.Name);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private bool CheckFields()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Name))
            {
                _dialogService.ShowWarning("Название курса не может быть пустым");
                return false;
            }

            return true;
        }
    }
}
