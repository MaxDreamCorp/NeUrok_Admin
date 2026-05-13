using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
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

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

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
                ViewModel.Client = e.First();
        }

        private void RemoveSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {

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
            card.ShowDialog();
        }
    }
}

