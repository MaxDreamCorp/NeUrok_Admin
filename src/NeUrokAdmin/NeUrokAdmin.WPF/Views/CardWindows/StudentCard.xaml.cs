using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Features.StudentOperations.Commands;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentCard.xaml
    /// </summary>
    public partial class StudentCard : Window
    {
        public StudentCardViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public StudentCard(IMediator mediator, NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
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
                    result = await CreateStudent();
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

        private async void SelectClientBtn_Click(object sender, RoutedEventArgs e)
        {
            var allClients = await _mediator.Send(new GetAllClientsQuery());

            var vm = new ClientsSelectorViewModel(allClients, ViewModel.Client);
            var selectorWindow = _navigationService.GetWindow<ClientsSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.ClientsSelected += SelectorWindow_ClientsSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_ClientsSelected(object? sender, List<ClientDTO> e)
        {
            if (e.Any())
            {
                ViewModel.Client = e.First();
                if (ViewModel.Subscriptions.Any())
                {
                    if (_dialogService.AskQuetion("Хотите ли вы очистить список абонементов?"))
                        ViewModel.Subscriptions.Clear();
                }
            }
        }

        private void RemoveSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SubsriptionsDg.SelectedItem is StudentSubscriptionDTO studentSubscriptionDTO)
            {
                if (!_dialogService.AskQuetion("Вы уверены, что хотите удалить запись?"))
                    return;

                ViewModel.Subscriptions.Remove(studentSubscriptionDTO);
            }
        }

        private void AddSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Client == null)
            {
                _dialogService.ShowWarning("Сначала выберите клиента");
                return;
            }
            var cardVM = new StudentSubscriptionCardViewModel(Enums.OperationType.Create, ViewModel.Client.ChildFullname);
            var card = _navigationService.GetWindow<StudentSubscriptionCard>();
            card.ViewModel = cardVM;
            card.StudentSubscriptionCreated += Card_StudentSubscriptionCreated;
            card.ShowDialog();
        }

        private void Card_StudentSubscriptionCreated(object? sender, StudentSubscriptionDTO e)
        {
            ViewModel.Subscriptions.Add(e);
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is StudentSubscriptionDTO studentSubscriptionDTO)
            {
                if (ViewModel.Client == null)
                {
                    _dialogService.ShowWarning("Сначала выберите клиента");
                    return;
                }
                var cardVM = new StudentSubscriptionCardViewModel(Enums.OperationType.Edit, ViewModel.Client.ChildFullname, studentSubscriptionDTO);
                var card = _navigationService.GetWindow<StudentSubscriptionCard>();
                card.ViewModel = cardVM;
                card.StudentSubscriptionEdited += (s, ss) =>
                {
                    int index = ViewModel.Subscriptions.IndexOf(studentSubscriptionDTO);
                    if (index >= 0)
                        ViewModel.Subscriptions[index] = ss;
                };
                card.ShowDialog();
            }
        }

        private async Task<bool> CreateStudent()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать нового ученика?"))
                return false;

            if (!CheckFields())
                return false;

            StudentDTO? dto;
            try
            {
                dto = ViewModel.GetStudentDTO();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }

            var cmd = new CreateStudentCommand(
                dto.Client.Id,
                dto.StudentSubscriptions);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private bool CheckFields()
        {
            if (ViewModel.Client == null)
            {
                _dialogService.ShowWarning("Клиент не выбран");
                return false;
            }
            if (!ViewModel.Subscriptions.Any())
            {
                _dialogService.ShowWarning("Ни один абонемент не выбран");
                return false;
            }
            return true;
        }
    }
}

