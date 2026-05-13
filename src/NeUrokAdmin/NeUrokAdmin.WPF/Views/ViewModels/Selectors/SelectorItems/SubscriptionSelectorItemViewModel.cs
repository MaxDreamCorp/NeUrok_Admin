using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class SubscriptionSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private SubscriptionDTO _subscription;

        [ObservableProperty]
        private bool _isSelected;

        public SubscriptionSelectorItemViewModel(SubscriptionDTO subscription, bool isSelected = false)
        {
            _subscription = subscription;
            _isSelected = isSelected;
        }
    }
}
