using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        public ClientsViewViewModel ViewModel { get; set; } = null!;
        private ClientCardViewModel _filterVM;

        public ClientsView(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
            _filterVM = new ClientCardViewModel(Enums.OperationType.Filter);
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        public async Task LoadData()
        {
            DataContext = ViewModel;
            await Clear();
        }

        private async void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ClientDTO client)
            {
                var cardVM = new ClientCardViewModel(Enums.OperationType.Edit, client);
                var card = _navigationService.GetWindow<ClientCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await PrintAll();
                    QuickSearch();
                }
            }
        }


        private async void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cardVM = new ClientCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<ClientCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                await Clear();
                QuickSearch();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuickSearch();
        }

        private async void FilterBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var card = _navigationService.GetWindow<ClientCard>();
            card.ViewModel = _filterVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                var searchDto = _filterVM.GetSearchDTO();
                var qry = new GetClientsByFilterQuery(searchDto);
                ViewModel.FilteredClients = new(await _mediator.Send(qry));
                ViewModel.IsFiltering = true;
                ResetDisplayedClientsAfterSearcing();
            }
        }

        private async void ClearBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Clear();
        }

        private async Task Clear()
        {
            ViewModel.FilteredClients = null;
            ViewModel.IsFiltering = false;
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
            _filterVM = new ClientCardViewModel(Enums.OperationType.Filter,
                maxId: ViewModel.AllClients.Any() ?
                ViewModel.AllClients.Max(c => c.Id) :
                null);
        }

        private async Task PrintAll()
        {
            var qry = new GetAllClientsQuery();
            ViewModel.AllClients = await _mediator.Send(qry);
            ViewModel.DisplayedClients = new(ViewModel.AllClients);
        }

        private void ResetDisplayedClientsAfterSearcing()
        {
            if (ViewModel.FilteredClients == null)
                ViewModel.DisplayedClients = new(ViewModel.AllClients);
            else
                ViewModel.DisplayedClients = new(ViewModel.FilteredClients);
        }

        private void QuickSearch()
        {
            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ResetDisplayedClientsAfterSearcing();
                return;
            }

            var initialList = ViewModel.FilteredClients?.ToList() ?? ViewModel.AllClients;

            ViewModel.DisplayedClients = new(initialList.Where(c =>
                c.Id.ToString().Contains(searchText) ||
                c.ChildFullname.ToLower().Contains(searchText) ||
                (c.BirthDate.HasValue && c.BirthDate.Value.ToString("dd.MM.yyyy").Contains(searchText)) ||
                c.RegistrationDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                (c.Grade.HasValue && c.Grade.Value.ToString().Contains(searchText)) ||
                c.Status.Status.ToLower().Contains(searchText) ||
                c.ParentName.ToLower().Contains(searchText) ||
                c.Phone.ToLower().Contains(searchText) ||
                c.WishedCoursesDisplay.ToLower().Contains(searchText) ||
                (c.Notes != null && c.Notes.ToLower().Contains(searchText)) ||
                (c.AdditionalPhones != null && c.AdditionalPhones.ToLower().Contains(searchText)))
                .ToList());
        }
    }
}
