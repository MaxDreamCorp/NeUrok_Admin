using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Commands;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionCard.xaml
    /// </summary>
    public partial class SubscriptionCard : Window
    {
        public SubscriptionCardViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public SubscriptionCard(NavigationService navigationService, IMediator mediator, IDialogService dialogService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
            _dialogService = dialogService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var qry = new GetAllClassesTypesQuery();
            var types = await _mediator.Send(qry);

            ViewModel.ClassesTypesDTO = types;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            switch (ViewModel.OperationType)
            {
                case Enums.OperationType.Create:
                    result = await CreateClient();
                    break;
                case Enums.OperationType.Read:
                    break;
                case Enums.OperationType.Edit:
                    //result = await UpdateClient();
                    break;
                case Enums.OperationType.Filter:
                    result = true;
                    break;
                default:
                    break;
            }

            if (result)
            {
                DialogResult = true;
                Close();
            }
        }

        private async Task<bool> CreateClient()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать новый абонемент?"))
                return false;

            if (!CheckFields())
                return false;

            try
            {
                var subscription = ViewModel.GetSubscriptionDTO();
                var cmd = new CreateSubscriptionCommand(
                    subscription.Name,
                    subscription.ClassesType.Id,
                    subscription.Cost,
                    subscription.ClassesAmount);

                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при создании абонемента: {ex.Message}");
                return false;
            }
        }

        private bool CheckFields()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Name))
            {
                _dialogService.ShowWarning("Поле 'Название' не может быть пустым.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ViewModel.ClassesType))
            {
                _dialogService.ShowWarning("Поле 'Тип занятий' не может быть пустым.");
                return false;
            }
            if (!ViewModel.Cost.HasValue || ViewModel.Cost <= 0)
            {
                _dialogService.ShowWarning("Поле 'Стоимость' должно быть положительным числом.");
                return false;
            }
            if (!ViewModel.ClassesAmount.HasValue || ViewModel.ClassesAmount <= 0)
            {
                _dialogService.ShowWarning("Поле 'Количество занятий' должно быть положительным числом.");
                return false;
            }
            return true;
        }
    }
}
