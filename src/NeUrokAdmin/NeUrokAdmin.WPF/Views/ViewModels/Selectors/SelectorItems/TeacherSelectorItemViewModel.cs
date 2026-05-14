using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class TeacherSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private TeacherDTO _teacher;

        [ObservableProperty]
        private bool _isSelected;

        public TeacherSelectorItemViewModel(TeacherDTO teacher, bool isSelected = false)
        {
            _teacher = teacher;
            _isSelected = isSelected;
        }
    }
}
