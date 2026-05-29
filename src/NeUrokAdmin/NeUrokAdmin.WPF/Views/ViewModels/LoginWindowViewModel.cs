using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class LoginWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private UpcomingBirthdaysDTO _upcomingBirthdays;

        [ObservableProperty]
        private ObservableCollection<ExpiringSubscriptionDTO> _expiringSubscriptions;

        public LoginWindowViewModel(UpcomingBirthdaysDTO upcomingBirthdays, ObservableCollection<ExpiringSubscriptionDTO> expiringSubscriptions)
        {
            _upcomingBirthdays = upcomingBirthdays;
            _expiringSubscriptions = expiringSubscriptions;
        }
    }
}
