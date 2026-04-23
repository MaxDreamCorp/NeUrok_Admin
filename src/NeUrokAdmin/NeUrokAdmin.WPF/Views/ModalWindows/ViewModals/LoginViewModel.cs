using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NeUrokAdmin.WPF.Services;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModals
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _statusMessage;

        private readonly NavigationService _navigationService;

        public LoginViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void LogIn(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;
                StatusMessage = $"{Login}: {password}";
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
