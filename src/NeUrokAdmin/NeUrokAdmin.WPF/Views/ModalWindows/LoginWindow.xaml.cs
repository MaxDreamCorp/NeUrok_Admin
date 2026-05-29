using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginWindowViewModel _viewModel = null!;
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;

        private bool _isLoaded;

        public LoginWindow(NavigationService navigationService, IMediator mediator, IDialogService dialogService, INotificationService notificationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;


#if DEBUG
            LoginInp.Text = "admin";
            PassInp.Password = "123";
            _dialogService = dialogService;
            _notificationService = notificationService;
#endif
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await SetNotifications();
            _isLoaded = true;
        }

        private void PassInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                placeholderText.Visibility = string.IsNullOrEmpty(pb.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await LogIn())
            {
                var mainWindow = _navigationService.GetWindow<MainWindow>();
                mainWindow.Show();
                Close();
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = _navigationService.GetWindow<RegistrationWindow>();
            regWindow.ShowDialog();
        }

        private async void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ExpiringSubscriptionDTO dto)
            {
                if (!_dialogService.AskQuetion("Чтобы открыть запись, необходимо авторизоваться.\n" +
                    "Хотите сделать попытку авторизации с введенными данными?"))
                    return;

                await OpenSubscription(dto);
            }
        }

        private async Task OpenSubscription(ExpiringSubscriptionDTO dto)
        {
            if (!await LogIn())
            {
                _dialogService.ShowWarning("Авторизация не удалась. Проверьте введенные данные.");
                return;
            }

            var studentDto = await _mediator.Send(new GetStudentByIdQuery(dto.StudentId));
            if (studentDto == null) return;
            var studentSubscriptionDTO = await _mediator.Send(new GetStudentSubscriptionByIdQuery(dto.SubscriptionId));
            if (studentSubscriptionDTO == null) return;

            var cardVM = new StudentCardViewModel(Enums.OperationType.Edit, studentDto);
            var card = _navigationService.GetWindow<StudentCard>();
            card.ViewModel = cardVM;
            card.Loaded += (s, args) =>
            {
                card.OpenSubscriptionToEdit(studentSubscriptionDTO);
            };

            card.ShowDialog();
            await SetNotifications();
        }

        private async Task<bool> LogIn()
        {
            if (string.IsNullOrEmpty(LoginInp.Text) || string.IsNullOrEmpty(PassInp.Password))
            {
                _dialogService.ShowWarning("Не все поля заполнены");
                return false;
            }
            var cmd = new LoginCommand(LoginInp.Text, PassInp.Password);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message, "Ошибка входа");
                return false;
            }
        }

        private async Task SetNotifications()
        {
            var upcomingBirthdays = await _mediator.Send(new GetUpcomingBirthdaysQuery());
            var expiringSubscriptions = (await _mediator.Send(new GetExpiringSubscriptionsQuery()))
                .OrderBy(it => it.FinishDate).ToList();
            var notPaidSubscriptions = (await _mediator.Send(new GetNotPaidSubscriptionsQuery()));
            _viewModel = new LoginWindowViewModel(upcomingBirthdays, new(expiringSubscriptions), new(notPaidSubscriptions));
            DataContext = _viewModel;

            if (_isLoaded) return;

            var msgs = upcomingBirthdays.TodayBirthdays.Select(it => $"{it.Key} - {it.Value}").ToList();
            if (msgs.Count > 0)
                _notificationService.ShowToastNotification(
                    "Дни рождения сегодня",
                    string.Join('\n', msgs));

            var subMsgs = expiringSubscriptions.Select(it => $"{it.ClientFullname} - {it.CourseName} ({it.FinishDate:dd.MM.yyyy})").ToList();
            if (subMsgs.Count > 0)
                _notificationService.ShowToastNotification(
                    "Истекающие абонементы",
                    string.Join('\n', subMsgs));
        }

    }
}
