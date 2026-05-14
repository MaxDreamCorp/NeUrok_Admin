using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class GroupsViewViewModel : ObservableObject
    {
        public List<GroupDTO> AllGroups { get; set; } = new();

        [ObservableProperty]
        private bool _isFiltering;

        [ObservableProperty]
        private ObservableCollection<GroupDTO>? _filteredGroups;

        [ObservableProperty]
        private ObservableCollection<GroupDTO> _displayedGroups = new();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;
    }
}
