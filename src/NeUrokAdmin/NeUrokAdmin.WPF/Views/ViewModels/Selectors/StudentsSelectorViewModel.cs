using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class StudentsSelectorViewModel : ObservableObject
    {
        public List<StudentSelectorItemViewModel> AllStudents { get; set; }

        [ObservableProperty]
        private List<StudentSelectorItemViewModel> _filteredStudents;

        public bool IsSingleton { get; set; }

        [ObservableProperty]
        private string _quickSearchText = string.Empty;

        public StudentsSelectorViewModel(List<StudentDTO> allStudents, List<StudentDTO>? selectedStudents)
        {
            var selectedIds = selectedStudents?.Select(c => c.Id).ToList() ?? new();
            AllStudents = new(allStudents.Select(c => new StudentSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredStudents = AllStudents;
        }

        public StudentsSelectorViewModel(List<StudentDTO> allStudents, StudentDTO? selectedStudent)
        {
            AllStudents = new(allStudents.Select(c => new StudentSelectorItemViewModel(c, selectedStudent != null && c.Id == selectedStudent.Id)));
            _filteredStudents = AllStudents;
            IsSingleton = true;
        }
    }
}
