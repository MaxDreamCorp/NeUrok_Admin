using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews
{
    public partial class CoursesViewViewModel : ObservableObject
    {
        public List<CourseDTO> AllCourses { get; set; } = new();

        [ObservableProperty]
        private ObservableCollection<CourseDTO> _displayedCourses = new();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;
    }
}
