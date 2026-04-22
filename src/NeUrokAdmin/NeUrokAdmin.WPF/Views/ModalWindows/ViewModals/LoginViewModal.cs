using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModals
{
    public partial class LoginViewModal : ObservableObject
    {
        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _statusMessage;

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

        }
    }
}
