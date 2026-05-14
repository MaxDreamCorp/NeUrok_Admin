using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class ClientSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private ClientDTO _client;

        [ObservableProperty]
        private bool _isSelected;

        public ClientSelectorItemViewModel(ClientDTO client, bool isSelected = false)
        {
            _client = client;
            _isSelected = isSelected;
        }
    }
}
