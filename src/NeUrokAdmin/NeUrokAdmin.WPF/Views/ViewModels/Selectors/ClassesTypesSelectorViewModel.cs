using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.ViewModels.Selectors
{
    public partial class ClassesTypesSelectorViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ClassesTypeSelectorItemViewModel> _allClassesTypes;

        public ClassesTypesSelectorViewModel(List<ClassesTypeDTO> allClassesTypes, List<ClassesTypeDTO>? selectedClassesTypes)
        {
            var selectedIds = selectedClassesTypes?.Select(c => c.Id).ToList() ?? new();
            _allClassesTypes = new(
                new(allClassesTypes.Select(c =>
                new ClassesTypeSelectorItemViewModel(c, selectedIds.Contains(c.Id)))));
        }
    }
}
