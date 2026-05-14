using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
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
        private readonly NavigationService _navigationService;

        public TeachersSelectorWindow(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void QuickSearchInp_TextChanged(object sender, TextChangedEventArgs e)
        {
            CopyFilterSelectedToAllTeachers();

            var searchText = QuickSearchInp.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.FilteredTeachers = ViewModel.AllTeachers;
                return;
            }

            ViewModel.FilteredTeachers = ViewModel.AllTeachers
                .Where(t => t.Teacher.Id.ToString().Contains(searchText) ||
                t.Teacher.Fullname.ToLower().Contains(searchText) ||
                t.Teacher.IndividualLessonsShare.ToString().Contains(searchText) ||
                (t.Teacher.Notes != null && t.Teacher.Notes.ToLower().Contains(searchText))).ToList();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new TeacherCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<TeacherCard>();
            card.ViewModel = vm;
            card.ShowDialog();
            if (card.DialogResult == true)
                await RefreeshList();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllTeachers();

            TeachersSelected?.Invoke(this, ViewModel.AllTeachers.Where(ci => ci.IsSelected)
                .Select(ci => ci.Teacher).ToList());
            Close();
        }

        private async void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is TeacherSelectorItemViewModel vm)
            {
                var teacher = ViewModel.AllTeachers.Find(ci => ci.Teacher.Id == vm.Teacher.Id)?.Teacher;
                if (teacher != null)
                {
                    var teacherVM = new TeacherCardViewModel(Enums.OperationType.Edit, teacher);
                    var card = _navigationService.GetWindow<TeacherCard>();
                    card.ViewModel = teacherVM;
                    card.ShowDialog();
                    if (card.DialogResult == true)
                        await RefreeshList();
                }
            }
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
