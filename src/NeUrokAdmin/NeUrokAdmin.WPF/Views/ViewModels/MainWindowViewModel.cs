using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<SideMenuItemViewModel> SideMenuItems { get; } = new();

        public event EventHandler<SideMenuItemViewModel>? SideMenuItemClicked;

        [ObservableProperty]
        private string _globalQuickSearchText = null!;

        public MainWindowViewModel()
        {
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Клиенты", Type = Enums.TabType.Clients, IconPath = "/Resources/Icons/ClientIcon.png", IsSelected = true });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Группы", Type = Enums.TabType.Groups, IconPath = "/Resources/Icons/GroupIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Ученики", Type = Enums.TabType.Students, IconPath = "/Resources/Icons/StudyIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Курсы", Type = Enums.TabType.Courses, IconPath = "/Resources/Icons/CourseIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Педагоги", Type = Enums.TabType.Teachers, IconPath = "/Resources/Icons/TeacherIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Абонементы", Type = Enums.TabType.Subscriptions, IconPath = "/Resources/Icons/TicketIcon.png" });
            SideMenuItems.Add(new SideMenuItemViewModel { Title = "Управление", Type = Enums.TabType.Subscriptions, IconPath = "/Resources/Icons/ManagementIcon.png" });

        }

        [RelayCommand]
        private void SelectTab(SideMenuItemViewModel selectedTab)
        {
            SideMenuItemClicked?.Invoke(this, selectedTab);
        }
    }

}
