using System.Windows;
using System.Windows.Controls;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModals;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly NavigationService _navigationService;

        public LoginWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;

            var vm = _navigationService.GetViewModel<LoginViewModel>();
            DataContext = vm;
        }

        private void PassInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
                placeholderText.Visibility = string.IsNullOrEmpty(pb.Password) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
