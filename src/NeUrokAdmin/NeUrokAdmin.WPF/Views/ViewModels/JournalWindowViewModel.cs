using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.UserControls;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class JournalWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private GroupDTO _group;

        public List<DateTime> Dates { get; init; }

        public List<StudentAttendancesDTO> StudentAttendances { get; init; }

        public JournalWindowViewModel(GroupDTO group, List<StudentAttendancesDTO> studentAttendances)
        {
            _group = group;
            Dates = group.Dates.Order().ToList();
            StudentAttendances = studentAttendances;
        }
    }
}
