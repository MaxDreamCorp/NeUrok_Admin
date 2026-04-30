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
        public ClientViewViewModel ViewModel { get; set; } = null!;

        public ClientsView(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        public async Task LoadData()
        {
            DataContext = ViewModel;
            await PrintAll();
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
                await PrintAll();
                QuickSearch();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuickSearch();
        }

        private void FilterBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private async void ClearBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.FilteredClients = null;
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
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
