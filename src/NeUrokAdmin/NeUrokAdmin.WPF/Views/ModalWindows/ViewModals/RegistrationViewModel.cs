using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.WPF.Interfaces;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModals
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _confirmPassword;

        [ObservableProperty]
        private string? _statusMessage;

        public event Action? Closing;

        private readonly IDialogService _dialogService;
        private readonly IMediator _mediator;

        public RegistrationViewModel(IDialogService dialogService, IMediator mediator)
        {
            _dialogService = dialogService;
            _mediator = mediator;
        }

        [RelayCommand]
        private void LogIn()
        {
            Closing?.Invoke();
        }

        [RelayCommand]
        private async Task Registration(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;

                if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(password))
                {
                    _dialogService.ShowWarning("Не все поля заполнены");
                    return;
                }

                if (!_dialogService.AskQuetion($"Вы уверены, что хотите зарегистрировать пользователя системы \"{Login}\"?"))
                    return;

                try
                {
                    var cmd = new RegistrateCommand(Login, password);
                    await _mediator.Send(cmd);
                    _dialogService.ShowMessage("Регистрация прошла успешно", "Успех");
                    Closing?.Invoke();
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError(ex.Message, "Ошибка регистрации");
                }
            }
        }
    }
}
