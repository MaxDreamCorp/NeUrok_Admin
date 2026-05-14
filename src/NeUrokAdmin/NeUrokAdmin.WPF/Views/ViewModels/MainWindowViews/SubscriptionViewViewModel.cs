using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews
{
    public partial class SubscriptionViewViewModel : ObservableObject
    {
        public List<SubscriptionDTO> AllSubscriptions { get; set; } = new();

        [ObservableProperty]
        private bool _isFiltering;

        [ObservableProperty]
        private ObservableCollection<SubscriptionDTO>? _filteredSubscriptions;

        [ObservableProperty]
        private ObservableCollection<SubscriptionDTO> _displayedSubscriptions = new ObservableCollection<SubscriptionDTO>();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;
    }
}
