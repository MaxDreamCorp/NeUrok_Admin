using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class ClientsSelectorViewModel : ObservableObject
    {
        public List<ClientSelectorItemViewModel> AllClients { get; set; }

        [ObservableProperty]
        private List<ClientSelectorItemViewModel> _filteredClients;

        public bool IsSingleton { get; set; }

        public ClientsSelectorViewModel(List<ClientDTO> allClients, List<ClientDTO>? selectedClients)
        {
            var selectedIds = selectedClients?.Select(c => c.Id).ToList() ?? new();
            AllClients = new(allClients.Select(c => new ClientSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredClients = AllClients;
        }

        public ClientsSelectorViewModel(List<ClientDTO> allClients, ClientDTO? selectedClient)
        {
            AllClients = new(allClients.Select(c => new ClientSelectorItemViewModel(c, selectedClient != null && c.Id == selectedClient.Id)));
            _filteredClients = AllClients;
            IsSingleton = true;
        }
    }
}
