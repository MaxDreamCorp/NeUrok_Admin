using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для ClientsSelectorWindow.xaml
    /// </summary>
    public partial class ClientsSelectorWindow : Window
    {
        public ClientsSelectorViewModel ViewModel { get; set; } = null!;
        public event EventHandler<List<ClientDTO>>? ClientsSelected;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public ClientsSelectorWindow(IMediator mediator, NavigationService navigationService)
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
            Close();
        }

        private void QuickSearchInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CopyFilterSelectedToAllClients();

            var searchText = QuickSearchInp.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.FilteredClients = ViewModel.AllClients;
                return;
            }


            ViewModel.FilteredClients = ViewModel.AllClients
                .Where(c =>
                c.Client.Id.ToString().Contains(searchText) ||
                c.Client.ChildFullname.ToLower().Contains(searchText) ||
                (c.Client.BirthDate.HasValue && c.Client.BirthDate.Value.ToString("dd.MM.yyyy").Contains(searchText)) ||
                c.Client.RegistrationDate.ToString("dd.MM.yyyy").Contains(searchText) ||
                (c.Client.Grade.HasValue && c.Client.Grade.Value.ToString().Contains(searchText)) ||
                c.Client.Status.Status.ToLower().Contains(searchText) ||
                c.Client.ParentName.ToLower().Contains(searchText) ||
                c.Client.Phone.ToLower().Contains(searchText) ||
                c.Client.WishedCoursesDisplay.ToLower().Contains(searchText) ||
                (c.Client.Notes != null && c.Client.Notes.ToLower().Contains(searchText)) ||
                (c.Client.AdditionalPhones != null && c.Client.AdditionalPhones.ToLower().Contains(searchText)))
                .ToList();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new ClientCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<ClientCard>();
            card.ViewModel = vm;
            card.ShowDialog();
            if (card.DialogResult == true)
                await RefreeshList();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllClients();

            ClientsSelected?.Invoke(this, ViewModel.AllClients.Where(ci => ci.IsSelected)
                .Select(ci => ci.Client).ToList());
            Close();
        }

        private async void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is ClientSelectorItemViewModel vm)
            {
                var client = ViewModel.AllClients.Find(ci => ci.Client.Id == vm.Client.Id)?.Client;
                if (client != null)
                {
                    var newVm = new ClientCardViewModel(Enums.OperationType.Edit, client);
                    var card = _navigationService.GetWindow<ClientCard>();
                    card.ViewModel = newVm;
                    card.ShowDialog();
                    if (card.DialogResult == true)
                        await RefreeshList();
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSingleton)
                return;

            if (sender is CheckBox checkBox && checkBox.DataContext is ClientSelectorItemViewModel vm)
            {
                var it = ViewModel.AllClients.Find(ci => ci.Client.Id == vm.Client.Id);
                if (it != null)
                {
                    foreach (var client in ViewModel.AllClients)
                    {
                        if (client.Client.Id != it.Client.Id)
                            client.IsSelected = false;
                    }
                }
            }
        }

        private async Task RefreeshList()
        {
            var selectedClients = ViewModel.AllClients.Where(ci => ci.IsSelected).Select(ci => ci.Client).ToList();

            var allClients = await _mediator.Send(new GetAllClientsQuery());
            var newVM = new ClientsSelectorViewModel(allClients, selectedClients);
            ViewModel = newVM;
            DataContext = ViewModel;
        }

        private void CopyFilterSelectedToAllClients()
        {
            foreach (var selectionItem in ViewModel.FilteredClients)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllClients.Find(ci => ci.Client.Id == selectionItem.Client.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }
        }
    }
}
