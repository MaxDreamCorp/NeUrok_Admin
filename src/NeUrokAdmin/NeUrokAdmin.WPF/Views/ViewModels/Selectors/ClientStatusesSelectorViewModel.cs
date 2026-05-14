using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class ClientStatusesSelectorViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ClientStatusSelectorItemViewModel> _allClientStatuses;

        public ClientStatusesSelectorViewModel(List<ClientStatusDTO> allClientStatuses, List<ClientStatusDTO>? selectedClientStatuses)
        {
            var selectedIds = selectedClientStatuses?.Select(c => c.Id).ToList() ?? new();
            _allClientStatuses = new(
                new(allClientStatuses.Select(c =>
                new ClientStatusSelectorItemViewModel(c, selectedIds.Contains(c.Id)))));
        }
    }
}
