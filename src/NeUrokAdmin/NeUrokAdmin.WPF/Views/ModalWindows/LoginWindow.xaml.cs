using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        private readonly IDialogService _dialogService;

        public LoginWindow(NavigationService navigationService, IMediator mediator, IDialogService dialogService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;


#if DEBUG
            LoginInp.Text = "admin";
            PassInp.Password = "123";
            _dialogService = dialogService;
#endif
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
                var mainWindow
            } catch (Exception ex)
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
