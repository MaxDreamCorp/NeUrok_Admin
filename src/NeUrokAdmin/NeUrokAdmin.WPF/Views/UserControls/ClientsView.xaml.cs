using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Commands;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        private readonly IDialogService _dialogService;
        public ClientsViewViewModel ViewModel { get; set; } = null!;
        private ClientCardViewModel _filterVM;

        public ClientsView(NavigationService navigationService, IMediator mediator, IDialogService dialogService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
            _filterVM = new ClientCardViewModel(Enums.OperationType.Filter);
            _dialogService = dialogService;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        public async Task LoadData()
        {
            DataContext = ViewModel;
            Properties.Settings.Default.UpdateYear = 2025;
            Properties.Settings.Default.Save();
            await Clear();
            await UpdateGrades();
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
                    await Refilter();
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
                await Refilter();
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
                ResetDisplayedClientsAfterSearching();
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

        private void ResetDisplayedClientsAfterSearching()
        {
            if (ViewModel.FilteredClients == null)
                ViewModel.DisplayedClients = new(ViewModel.AllClients);
            else
                ViewModel.DisplayedClients = new(ViewModel.FilteredClients);
        }

        private async Task Refilter()
        {
            if (ViewModel.IsFiltering)
            {
                var searchDto = _filterVM.GetSearchDTO();
                var qry = new GetClientsByFilterQuery(searchDto);
                ViewModel.FilteredClients = new(await _mediator.Send(qry));
                ResetDisplayedClientsAfterSearching();
            }
        }

        private void QuickSearch()
        {
            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ResetDisplayedClientsAfterSearching();
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

        private async Task UpdateGrades()
        {
            if (DateTime.Today < new DateTime(DateTime.Today.Year, 9, 1))
                return;

            if (Properties.Settings.Default.UpdateYear >= DateTime.Today.Year)
                return;

            if (!_dialogService.AskQuetion("Обновить классы?"))
                return;

            await _mediator.Send(new UpdateGradesCommand());

            Properties.Settings.Default.UpdateYear = DateTime.Today.Year;
            Properties.Settings.Default.Save();

            await PrintAll();
        }
    }
}
