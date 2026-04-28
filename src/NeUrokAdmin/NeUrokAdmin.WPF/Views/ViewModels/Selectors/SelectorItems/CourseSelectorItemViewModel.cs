using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class CourseSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private CourseDTO _course;

        [ObservableProperty]
        private bool _isSelected;

        public CourseSelectorItemViewModel(CourseDTO course, bool isSelected = false)
        {
            _course = course;
            _isSelected = isSelected;
        }
    }
}
