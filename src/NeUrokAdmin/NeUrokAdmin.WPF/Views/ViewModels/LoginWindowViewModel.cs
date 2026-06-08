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

        [ObservableProperty]
        private ObservableCollection<ExpiringSubscriptionDTO> _notPaidSubscriptions;

        public LoginWindowViewModel(UpcomingBirthdaysDTO upcomingBirthdays, ObservableCollection<ExpiringSubscriptionDTO> expiringSubscriptions, ObservableCollection<ExpiringSubscriptionDTO> notPaidSubscriptions)
        {
            _upcomingBirthdays = upcomingBirthdays;
            _expiringSubscriptions = expiringSubscriptions;
            _notPaidSubscriptions = notPaidSubscriptions;
        }
    }
}
