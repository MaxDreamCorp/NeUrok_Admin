using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class JournalWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private GroupDTO _group;

        public List<DateTime> Dates { get; init; }

        public List<StudentAttendancesDTO> StudentAttendances { get; init; }

        public JournalWindowViewModel(GroupDTO group, List<DateTime> dates, List<StudentAttendancesDTO> studentAttendances)
        {
            _group = group;
            Dates = dates;
            StudentAttendances = studentAttendances;
        }
    }
}
