using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews
{
    public partial class ClientsViewViewModel : ObservableObject
    {
        public List<ClientDTO> AllClients { get; set; } = new();

        [ObservableProperty]
        private bool _isFiltering;

        [ObservableProperty]
        private ObservableCollection<ClientDTO>? _filteredClients;

        [ObservableProperty]
        private ObservableCollection<ClientDTO> _displayedClients = new ObservableCollection<ClientDTO>();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;

    }
}
