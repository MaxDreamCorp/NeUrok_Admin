using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class LoginWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private UpcomingBirthdaysDTO _upcomingBirthdays;

        public LoginWindowViewModel(UpcomingBirthdaysDTO upcomingBirthdays)
        {
            _upcomingBirthdays = upcomingBirthdays;
        }
    }
}
