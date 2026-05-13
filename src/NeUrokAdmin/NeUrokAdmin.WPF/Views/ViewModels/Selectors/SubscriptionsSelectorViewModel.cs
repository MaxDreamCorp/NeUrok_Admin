using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class SubscriptionsSelectorViewModel : ObservableObject
    {
        public List<SubscriptionSelectorItemViewModel> AllSubscriptions { get; set; }

        [ObservableProperty]
        private List<SubscriptionSelectorItemViewModel> _filteredSubscriptions;

        public bool IsSingleton { get; set; }

        public SubscriptionsSelectorViewModel(List<SubscriptionDTO> allSubscriptions, List<SubscriptionDTO>? selectedSubscriptions)
        {
            var selectedIds = selectedSubscriptions?.Select(c => c.Id).ToList() ?? new();
            AllSubscriptions = new(allSubscriptions.Select(c => new SubscriptionSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredSubscriptions = AllSubscriptions;
        }

        public SubscriptionsSelectorViewModel(List<SubscriptionDTO> allSubscriptions, SubscriptionDTO? selectedSubscription)
        {
            AllSubscriptions = new(allSubscriptions.Select(c => new SubscriptionSelectorItemViewModel(c, selectedSubscription != null && c.Id == selectedSubscription.Id)));
            _filteredSubscriptions = AllSubscriptions;
            IsSingleton = true;
        }
    }
}
