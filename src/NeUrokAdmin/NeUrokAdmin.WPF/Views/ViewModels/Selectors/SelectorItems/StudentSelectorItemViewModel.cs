using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class StudentSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private StudentDTO _student;

        [ObservableProperty]
        private bool _isSelected;

        public StudentSelectorItemViewModel(StudentDTO student, bool isSelected = false)
        {
            _student = student;
            _isSelected = isSelected;
        }
    }
}
