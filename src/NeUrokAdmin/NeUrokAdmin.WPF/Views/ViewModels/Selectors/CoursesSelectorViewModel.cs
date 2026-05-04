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

        public bool IsSingleton { get; set; }

        public CoursesSelectorViewModel(List<CourseDTO> allCourses, List<CourseDTO>? selectedCourses)
        {
            var selectedIds = selectedCourses?.Select(c => c.Id).ToList() ?? new();
            AllCourses = new(allCourses.Select(c => new CourseSelectorItemViewModel(c, selectedIds.Contains(c.Id))));
            _filteredCourses = AllCourses;
        }

        public CoursesSelectorViewModel(List<CourseDTO> allCourses, CourseDTO? selectedCourse)
        {
            AllCourses = new(allCourses.Select(c => new CourseSelectorItemViewModel(c, selectedCourse != null && c.Id == selectedCourse.Id)));
            _filteredCourses = AllCourses;
            IsSingleton = true;
        }
    }
}
