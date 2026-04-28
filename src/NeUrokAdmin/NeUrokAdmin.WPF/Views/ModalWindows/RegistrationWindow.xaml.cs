using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.Authorization.Commands;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IMediator _mediator;

        public RegistrationWindow(NavigationService navigationService, IDialogService dialogService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _dialogService = dialogService;
            _mediator = mediator;
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

        private async void RegistrateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginInp.Text) || string.IsNullOrEmpty(PassInp.Password))
            {
                _dialogService.ShowWarning("Не все поля заполнены");
                return;
            }

            if (!_dialogService.AskQuetion($"Вы уверены, что хотите зарегистрировать пользователя {LoginInp.Text}?"))
                return;

            var cmd = new RegistrateCommand(LoginInp.Text, PassInp.Password);

            try
            {
                await _mediator.Send(cmd);
                _dialogService.ShowMessage($"Пользователь {LoginInp.Text} успешно зарегистрирован", "Успех");
                Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message, "Ошибка входа");
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
