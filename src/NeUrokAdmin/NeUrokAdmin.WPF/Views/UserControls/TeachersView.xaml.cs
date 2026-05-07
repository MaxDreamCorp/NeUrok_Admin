using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TeachersView.xaml
    /// </summary>
    public partial class TeachersView : UserControl
    {
        public TeachersViewViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public TeachersView(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
        }

        public async Task LoadData()
        {
            DataContext = ViewModel;
            await Clear();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardVM = new TeacherCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<TeacherCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                await Clear();
                QuickSearch();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuickSearch();
        }

        private async void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            await Clear();
        }

        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is TeacherDTO teacher)
            {
                var cardVM = new TeacherCardViewModel(Enums.OperationType.Edit, teacher);
                var card = _navigationService.GetWindow<TeacherCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await Clear();
                    QuickSearch();
                }
            }
        }

        private async Task Clear()
        {
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
        }

        private async Task PrintAll()
        {
            var qry = new GetAllTeachersQuery();
            ViewModel.AllTeachers = await _mediator.Send(qry);
            ViewModel.DisplayedTeachers = new(ViewModel.AllTeachers);
        }

        private void QuickSearch()
        {
            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.DisplayedTeachers = new(ViewModel.AllTeachers);
                return;
            }

            ViewModel.DisplayedTeachers = new(ViewModel.AllTeachers.Where(t =>
                t.Id.ToString().Contains(searchText) ||
                t.Fullname.ToLower().Contains(searchText) ||
                t.IndividualLessonsShare.ToString().Contains(searchText) ||
                (t.Notes != null && t.Notes.ToLower().Contains(searchText))));
        }
    }
}
