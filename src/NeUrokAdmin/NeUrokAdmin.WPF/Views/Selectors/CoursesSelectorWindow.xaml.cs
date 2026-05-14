using System.Windows;
using System.Windows.Controls;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для CoursesSelectorWindow.xaml
    /// </summary>
    public partial class CoursesSelectorWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        public event EventHandler<List<CourseDTO>>? CoursesSelected;

        public CoursesSelectorViewModel ViewModel { get; set; } = null!;

        public CoursesSelectorWindow(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
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
            CopyFilterSelectedToAllCourses();

            var searchText = QuickSearchInp.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.FilteredCourses = ViewModel.AllCourses;
                return;
            }

            ViewModel.FilteredCourses = ViewModel.AllCourses
                .Where(ci => ci.Course.Id.ToString().Contains(searchText) ||
                ci.Course.Name.ToLower().Contains(searchText)).ToList();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllCourses();

            CoursesSelected?.Invoke(this, ViewModel.AllCourses.Where(ci => ci.IsSelected)
                .Select(ci => ci.Course).ToList());
            Close();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new CourseCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<CourseCard>();
            card.ViewModel = vm;
            card.ShowDialog();
            if (card.DialogResult == true)
                await RefreeshList();
        }

        private void CopyFilterSelectedToAllCourses()
        {
            foreach (var selectionItem in ViewModel.FilteredCourses)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllCourses.Find(ci => ci.Course.Id == selectionItem.Course.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }
        }

        private async void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.Content is CourseSelectorItemViewModel vm)
            {
                var dto = vm.Course;

                var cardVm = new CourseCardViewModel(Enums.OperationType.Edit, dto);

                var editCard = _navigationService.GetWindow<CourseCard>();
                editCard.ViewModel = cardVm;
                editCard.ShowDialog();
                if (editCard.DialogResult == true)
                    await RefreeshList();
            }
        }

        private async Task RefreeshList()
        {
            var selectedCourses = ViewModel.AllCourses.Where(ci => ci.IsSelected).Select(ci => ci.Course).ToList();

            var allCourses = await _mediator.Send(new GetAllCoursesQuery());
            var newVM = new CoursesSelectorViewModel(allCourses, selectedCourses);
            ViewModel = newVM;
            DataContext = ViewModel;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSingleton)
                return;

            if (sender is CheckBox checkBox && checkBox.DataContext is CourseSelectorItemViewModel vm)
            {
                var it = ViewModel.AllCourses.Find(ci => ci.Course.Id == vm.Course.Id);
                if (it != null)
                {
                    foreach (var selItem in ViewModel.AllCourses)
                    {
                        if (selItem.Course.Id != it.Course.Id)
                            selItem.IsSelected = false;
                    }
                }
            }
        }
    }
}