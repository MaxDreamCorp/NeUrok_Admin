using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Controls
{
    public partial class JournalCellViewModel : ObservableObject
    {
        [ObservableProperty]
        private AttendanceDTO _attendance;

        public StudentDTO Student { get; init; }
        public StudentSubscriptionDTO StudentSubscription { get; init; }
        public GroupDTO? Group { get; init; }

        public JournalCellViewModel(AttendanceDTO attendance, StudentDTO student, StudentSubscriptionDTO studentSubscription, GroupDTO? group)
        {
            _attendance = attendance;
            Student = student;
            Group = group;
            StudentSubscription = studentSubscription;
        }
    }
}
