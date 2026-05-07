using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.SubscriptionOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SubscriptionView.xaml
    /// </summary>
    public partial class SubscriptionView : UserControl
    {
        public SubscriptionViewViewModel ViewModel { get; set; } = null!;

        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        private SubscriptionCardViewModel _filterVM;

        public SubscriptionView(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
            _filterVM = new SubscriptionCardViewModel(Enums.OperationType.Filter);
        }


        public async Task LoadData()
        {
            DataContext = ViewModel;
            await Clear();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardVM = new SubscriptionCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<SubscriptionCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                await Clear();
                //await Refilter();
                //QuickSearch();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is SubscriptionDTO subscription)
            {
                var cardVM = new SubscriptionCardViewModel(Enums.OperationType.Edit, subscription);
                var card = _navigationService.GetWindow<SubscriptionCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await PrintAll();
                    //await Refilter();
                    //QuickSearch();
                }
            }
        }

        private async Task Clear()
        {
            ViewModel.FilteredSubscriptions = null;
            ViewModel.IsFiltering = false;
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
            _filterVM = new SubscriptionCardViewModel(Enums.OperationType.Filter,
                maxId: ViewModel.AllSubscriptions.Any() ?
                ViewModel.AllSubscriptions.Max(c => c.Id) :
                null);
        }

        private async Task PrintAll()
        {
            var qry = new GetAllSubscriptionsQuery();
            ViewModel.AllSubscriptions = await _mediator.Send(qry);
            ViewModel.DisplayedSubscriptions = new(ViewModel.AllSubscriptions);
        }
    }
}
