using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class SideMenuItemViewModel : ObservableObject
    {
        public string Title { get; init; } = null!;
        public string IconPath { get; init; } = null!;
        public TabType Type { get; set; }

        [ObservableProperty]
        private bool _isSelected;
    }
}
