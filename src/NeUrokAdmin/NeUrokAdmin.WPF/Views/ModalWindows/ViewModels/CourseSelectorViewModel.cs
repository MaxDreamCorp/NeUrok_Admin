using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.Elements.ViewModels;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModels
{
    public partial class CourseSelectorViewModel : BaseMultiplySelectorViewModel
    {
        private readonly List<CourseDTO> _fullList;

        [ObservableProperty]
        private ObservableCollection<SelectableItem<CourseDTO>> _filteredItems;

        public override IEnumerable<object> AllItems => throw new NotImplementedException();

        public CourseSelectorViewModel(List<CourseDTO> allCourses, List<CourseDTO> selectedCourses)
        {
            _fullList = allCourses;

            var selectedIds = selectedCourses.Select(c => c.Id).ToHashSet();

            FilteredItems = new(
                allCourses.Select(c => new SelectableItem<CourseDTO>(c, selectedIds.Contains(c.Id))));
        }

        public override void Back()
        {
            throw new NotImplementedException();
        }

        public override void QuickSearch()
        {
            throw new NotImplementedException();
        }

        public override void Select()
        {
            throw new NotImplementedException();
        }
    }
}
