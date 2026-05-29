using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Features.StudentSubscriptionOperations.Queries;
using NeUrokAdmin.Application.Interfaces;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels;

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
            var upcomingBirthdays = await _mediator.Send(new GetUpcomingBirthdaysQuery());
            var expiringSubscriptions = (await _mediator.Send(new GetExpiringSubscriptionsQuery()))
                .OrderBy(it => it.FinishDate).ToList();
            _viewModel = new LoginWindowViewModel(upcomingBirthdays, new(expiringSubscriptions));
            DataContext = _viewModel;

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

        private void PassInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                placeholderText.Visibility = string.IsNullOrEmpty(pb.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginInp.Text) || string.IsNullOrEmpty(PassInp.Password))
            {
                _dialogService.ShowWarning("Не все поля заполнены");
                return;
            }
            var cmd = new LoginCommand(LoginInp.Text, PassInp.Password);

            try
            {
                await _mediator.Send(cmd);
                var mainWindow = _navigationService.GetWindow<MainWindow>();
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message, "Ошибка входа");
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = _navigationService.GetWindow<RegistrationWindow>();
            regWindow.ShowDialog();
        }
    }
}
