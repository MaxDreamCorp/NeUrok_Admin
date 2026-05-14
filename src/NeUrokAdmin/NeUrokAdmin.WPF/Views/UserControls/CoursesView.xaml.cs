using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для CoursesView.xaml
    /// </summary>
    public partial class CoursesView : UserControl
    {
        public CoursesViewViewModel ViewModel { get; set; } = null!;

        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;

        public CoursesView(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
        }

        public async Task LoadData()
        {
            DataContext = ViewModel;
            await PrintAll();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardVM = new CourseCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<CourseCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                await QuickSearch();
            }
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await QuickSearch();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is CourseDTO course)
            {
                var cardVM = new CourseCardViewModel(Enums.OperationType.Edit, course);
                var card = _navigationService.GetWindow<CourseCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await QuickSearch();
                }
            }
        }

        private void Clear()
        {
            ViewModel.QuickSearchText = string.Empty;
        }

        private async Task PrintAll()
        {
            var qry = new GetAllCoursesQuery();
            ViewModel.AllCourses = await _mediator.Send(qry);
            ViewModel.DisplayedCourses = new(ViewModel.AllCourses);
        }

        private async Task QuickSearch()
        {
            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                await PrintAll();
                return;
            }

            ViewModel.DisplayedCourses = new(ViewModel.AllCourses
                .Where(ci => ci.Id.ToString().Contains(searchText) ||
                ci.Name.ToLower().Contains(searchText)).ToList());
        }
    }
}
