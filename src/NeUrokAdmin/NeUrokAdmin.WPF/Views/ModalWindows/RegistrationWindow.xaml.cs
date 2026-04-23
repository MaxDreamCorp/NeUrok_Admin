using System.Windows;
using System.Windows.Controls;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ModalWindows.ViewModals;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly NavigationService _navigationService;

        public RegistrationWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;

            var vm = _navigationService.GetViewModel<RegistrationViewModel>();
            vm.Closing += Close;
            DataContext = vm;
        }

        private void PassInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                placeholderText.Visibility = string.IsNullOrEmpty(pb.Password) ? Visibility.Visible : Visibility.Collapsed;

                if (pb.Password != ConfirmPassInp.Password)
                {
                    RegistrateBtn.Opacity = 0.5;
                    RegistrateBtn.IsEnabled = false;
                }
                else
                {
                    RegistrateBtn.Opacity = 1;
                    RegistrateBtn.IsEnabled = true;
                }
            }
        }

        private void ConfirmPassInp_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                cplaceholderText.Visibility = string.IsNullOrEmpty(pb.Password) ? Visibility.Visible : Visibility.Collapsed;

                if (pb.Password != PassInp.Password)
                {
                    RegistrateBtn.Opacity = 0.5;
                    RegistrateBtn.IsEnabled = false;
                }
                else
                {
                    RegistrateBtn.Opacity = 1;
                    RegistrateBtn.IsEnabled = true;
                }
            }
        }
    }
}
