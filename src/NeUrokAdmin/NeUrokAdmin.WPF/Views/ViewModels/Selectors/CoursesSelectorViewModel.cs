using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class CoursesSelectorViewModel : ObservableObject
    {
        public List<CourseSelectorItemViewModel> AllCourses { get; set; }

        [ObservableProperty]
        private List<CourseSelectorItemViewModel> _filteredCourses;

        public CoursesSelectorViewModel(List<CourseDTO> allCourses, List<CourseDTO>? selectedCourses)
        {
            var selectedIds = selectedCourses?.Select(c => c.Id).ToList() ?? new();
            AllCourses = new(allCourses.Select(c => new CourseSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredCourses = AllCourses;
        }
    }
}
