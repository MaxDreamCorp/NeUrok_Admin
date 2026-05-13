using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionsSelectorWindow.xaml
    /// </summary>
    public partial class SubscriptionsSelectorWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;

        public event EventHandler<List<SubscriptionDTO>>? SubscriptionsSelected;

        public SubscriptionsSelectorViewModel ViewModel { get; set; } = null!;

        public SubscriptionsSelectorWindow(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void QuickSearchInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CopyFilterSelectedToAllSubscriptions();

            var searchText = QuickSearchInp.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.FilteredSubscriptions = ViewModel.AllSubscriptions;
                return;
            }

            ViewModel.FilteredSubscriptions = ViewModel.AllSubscriptions
                .Where(s =>
                s.Subscription.Id.ToString().Contains(searchText) ||
                s.Subscription.Name.ToLower().Contains(searchText) ||
                s.Subscription.ClassesType.Type.ToLower().Contains(searchText) ||
                s.Subscription.Cost.ToString().Contains(searchText) ||
                s.Subscription.ClassesAmount.ToString().Contains(searchText)).ToList();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new SubscriptionCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<SubscriptionCard>();
            card.ViewModel = vm;
            card.ShowDialog();
            if (card.DialogResult == true)
                await RefreeshList();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllSubscriptions();

            SubscriptionsSelected?.Invoke(this, ViewModel.AllSubscriptions.Where(ci => ci.IsSelected)
                .Select(ci => ci.Subscription).ToList());
            Close();
        }

        private async void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.Content is SubscriptionSelectorItemViewModel vm)
            {
                var dto = vm.Subscription;

                var cardVm = new SubscriptionCardViewModel(Enums.OperationType.Edit, dto);

                var editCard = _navigationService.GetWindow<SubscriptionCard>();
                editCard.ViewModel = cardVm;
                editCard.ShowDialog();
                if (editCard.DialogResult == true)
                    await RefreeshList();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSingleton)
                return;

            if (sender is CheckBox checkBox && checkBox.DataContext is SubscriptionSelectorItemViewModel vm)
            {
                var it = ViewModel.AllSubscriptions.Find(ci => ci.Subscription.Id == vm.Subscription.Id);
                if (it != null)
                {
                    foreach (var selItem in ViewModel.AllSubscriptions)
                    {
                        if (selItem.Subscription.Id != it.Subscription.Id)
                            selItem.IsSelected = false;
                    }
                }
            }
        }

        private void CopyFilterSelectedToAllSubscriptions()
        {
            foreach (var selectionItem in ViewModel.FilteredSubscriptions)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllSubscriptions.Find(ci => ci.Subscription.Id == selectionItem.Subscription.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }
        }

        private async Task RefreeshList()
        {
            var selectedSubscriptions = ViewModel.AllSubscriptions.Where(ci => ci.IsSelected).Select(ci => ci.Subscription).ToList();

            var allSubscriptions = await _mediator.Send(new GetAllSubscriptionsQuery());
            var newVM = new SubscriptionsSelectorViewModel(allSubscriptions, selectedSubscriptions);
            ViewModel = newVM;
            DataContext = ViewModel;
        }
    }
}
