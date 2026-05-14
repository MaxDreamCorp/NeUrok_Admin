using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems
{
    public partial class ClassesTypeSelectorItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private ClassesTypeDTO _classesType;

        [ObservableProperty]
        private bool _isSelected;

        public ClassesTypeSelectorItemViewModel(ClassesTypeDTO classesType, bool isSelected = false)
        {
            _classesType = classesType;
            _isSelected = isSelected;
        }
    }
}
