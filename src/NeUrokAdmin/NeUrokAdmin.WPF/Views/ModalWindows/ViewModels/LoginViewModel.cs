using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _statusMessage;

        public event Action? Closing;

        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IMediator _mediator;

        public LoginViewModel(NavigationService navigationService, IDialogService dialogService, IMediator mediator)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _mediator = mediator;
        }

        [RelayCommand]
        private async Task LogIn(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;

                if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(password))
                {
                    _dialogService.ShowWarning("Не все поля заполнены");
                    return;
                }

                try
                {
                    var cmd = new LoginCommand(Login, password);
                    await _mediator.Send(cmd);
                    var mainWindow = _navigationService.GetWindow<MainWindow>();
                    mainWindow.Show();
                    Closing?.Invoke();
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError(ex.Message, "Ошибка входа");
                }
            }
        }

        [RelayCommand]
        private void Registration()
        {
            var window = _navigationService.GetWindow<RegistrationWindow>();
            window.ShowDialog();
        }
    }
}
