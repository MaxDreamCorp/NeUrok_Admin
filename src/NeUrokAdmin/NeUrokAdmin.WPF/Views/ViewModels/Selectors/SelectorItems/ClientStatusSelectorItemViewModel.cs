using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class ClientStatusSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private ClientStatusDTO _clientStatus;

        [ObservableProperty]
        private bool _isSelected;

        public ClientStatusSelectorItemViewModel(ClientStatusDTO clientStatus, bool isSelected = false)
        {
            _clientStatus = clientStatus;
            _isSelected = isSelected;
        }
    }
}
