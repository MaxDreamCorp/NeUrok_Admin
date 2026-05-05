using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для TeachersSelectorWindow.xaml
    /// </summary>
    public partial class TeachersSelectorWindow : Window
    {
        public TeacherSelectorViewModel ViewModel { get; set; } = null!;
        public event EventHandler<List<TeacherDTO>>? TeachersSelected;

        private readonly IMediator _mediator;

        public TeachersSelectorWindow(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QuickSearchInp_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllTeachers();

            TeachersSelected?.Invoke(this, ViewModel.AllTeachers.Where(ci => ci.IsSelected)
                .Select(ci => ci.Teacher).ToList());
            Close();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSingleton)
                return;

            if (sender is CheckBox checkBox && checkBox.DataContext is TeacherSelectorItemViewModel vm)
            {
                var it = ViewModel.AllTeachers.Find(ci => ci.Teacher.Id == vm.Teacher.Id);
                if (it != null)
                {
                    foreach (var teacher in ViewModel.AllTeachers)
                    {
                        if (teacher.Teacher.Id != it.Teacher.Id)
                            teacher.IsSelected = false;
                    }
                }
            }
        }

        private async Task RefreeshList()
        {
            var selectedTeachers = ViewModel.AllTeachers.Where(ci => ci.IsSelected).Select(ci => ci.Teacher).ToList();

            var allTeachers = await _mediator.Send(new GetAllTeachersQuery());
            var newVM = new TeacherSelectorViewModel(allTeachers, selectedTeachers);
            ViewModel = newVM;
            DataContext = ViewModel;
        }

        private void CopyFilterSelectedToAllTeachers()
        {
            foreach (var selectionItem in ViewModel.FilteredTeachers)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllTeachers.Find(ci => ci.Teacher.Id == selectionItem.Teacher.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }
        }

    }
}
