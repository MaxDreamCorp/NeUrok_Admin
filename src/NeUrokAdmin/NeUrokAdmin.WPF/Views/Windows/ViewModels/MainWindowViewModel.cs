using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Elements.ViewModels;

namespace NeUrokAdmin.WPF.Views.Windows.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;

        public ObservableCollection<SideMenuItemViewModel> SideMenuItems { get; } = new();

        [ObservableProperty]
        private BaseDataViewModel _currentPage;

        [ObservableProperty]
        private string _globalQuickSearchText;

        public MainWindowViewModel(NavigationService navigationService)
        {
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Клиенты", Type = Enums.TabType.Clients, IconPath = "/Resources/Icons/ClientIcon.png", IsSelected = true });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Ученики", Type = Enums.TabType.Students, IconPath = "/Resources/Icons/StudyIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Группы", Type = Enums.TabType.Groups, IconPath = "/Resources/Icons/GroupIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Абонементы", Type = Enums.TabType.Subscriptions, IconPath = "/Resources/Icons/TicketIcon.png" });
            _navigationService = navigationService;


        }

        [RelayCommand]
        private void SelectTab(SideMenuItemViewModel selectedTab)
        {
            foreach (var item in SideMenuItems)
                item.IsSelected = false;
            selectedTab.IsSelected = true;
        }
    }
}
