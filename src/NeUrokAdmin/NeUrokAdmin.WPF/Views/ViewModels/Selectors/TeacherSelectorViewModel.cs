using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class TeacherSelectorViewModel : ObservableObject
    {
        public List<TeacherSelectorItemViewModel> AllTeachers { get; set; }

        [ObservableProperty]
        private List<TeacherSelectorItemViewModel> _filteredTeachers;

        public bool IsSingleton { get; set; }

        public TeacherSelectorViewModel(List<TeacherDTO> allTeachers, List<TeacherDTO> selectedTeachers)
        {
            var selectedIds = selectedTeachers.Select(c => c.Id).ToList();
            AllTeachers = new(allTeachers.Select(c => new TeacherSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredTeachers = AllTeachers;
        }

        public TeacherSelectorViewModel(List<TeacherDTO> allTeachers, TeacherDTO? selectedTeacher)
        {
            AllTeachers = new(allTeachers.Select(c => new TeacherSelectorItemViewModel(c, selectedTeacher != null && c.Id == selectedTeacher.Id)));
            _filteredTeachers = AllTeachers;
            IsSingleton = true;
        }
    }
}
