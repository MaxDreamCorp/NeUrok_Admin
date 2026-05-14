using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews
{
    public partial class TeachersViewViewModel : ObservableObject
    {
        public List<TeacherDTO> AllTeachers { get; set; } = new();

        [ObservableProperty]
        private ObservableCollection<TeacherDTO> _displayedTeachers = new ObservableCollection<TeacherDTO>();

        [ObservableProperty]
        private string _quickSearchText = string.Empty;
    }
}
