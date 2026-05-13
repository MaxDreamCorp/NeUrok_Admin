using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentSubscriptionCard.xaml
    /// </summary>
    public partial class StudentSubscriptionCard : Window
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public StudentSubscriptionCardViewModel ViewModel { get; set; } = null!;

        public StudentSubscriptionCard(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void SelectSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {
            var allSubscriptions = await _mediator.Send(new GetAllSubscriptionsQuery());

            var vm = new SubscriptionsSelectorViewModel(allSubscriptions, ViewModel.Subscription);
            var selectorWindow = _navigationService.GetWindow<SubscriptionsSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.SubscriptionsSelected += SelectorWindow_SubscriptionsSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_SubscriptionsSelected(object? sender, List<SubscriptionDTO> e)
        {
            if (e.Any())
                ViewModel.Subscription = e.First();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
