using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews
{
    public partial class StudentsViewViewModel : ObservableObject
    {
        public List<StudentDTO> AllStudents { get; set; } = new();

        [ObservableProperty]
        private bool _isFiltering;

        [ObservableProperty]
        private ObservableCollection<StudentDTO>? _filteredStudents;

        [ObservableProperty]
        private ObservableCollection<StudentDTO> _displayedStudents = new ObservableCollection<StudentDTO>();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;
    }
}
