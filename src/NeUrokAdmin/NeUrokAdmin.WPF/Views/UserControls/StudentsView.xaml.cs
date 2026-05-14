using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StudentsView.xaml
    /// </summary>
    public partial class StudentsView : UserControl
    {
        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;
        public StudentsViewViewModel ViewModel { get; set; } = null!;

        public StudentsView(NavigationService navigationService, IMediator mediator)
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

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardVM = new StudentCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<StudentCard>();
            card.ViewModel = cardVM;
            card.ShowDialog();
            if (card.DialogResult == true)
            {
                await PrintAll();
                QuickSearch();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuickSearch();
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
        }

        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is StudentDTO student)
            {
                var cardVM = new StudentCardViewModel(Enums.OperationType.Edit, student);
                var card = _navigationService.GetWindow<StudentCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await PrintAll();
                    //await Refilter();
                    QuickSearch();
                }
            }
        }

        private async Task PrintAll()
        {
            var qry = new GetAllStudentsQuery();
            ViewModel.AllStudents = await _mediator.Send(qry);
            ViewModel.DisplayedStudents = new(ViewModel.AllStudents);
        }

        private void ResetDisplayedStudentsAfterSearching()
        {
            if (ViewModel.FilteredStudents == null)
                ViewModel.DisplayedStudents = new(ViewModel.AllStudents);
            else
                ViewModel.DisplayedStudents = new(ViewModel.FilteredStudents);
        }

        private void QuickSearch()
        {
            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ResetDisplayedStudentsAfterSearching();
                return;
            }

            var initialList = ViewModel.FilteredStudents?.ToList() ?? ViewModel.AllStudents;

            ViewModel.DisplayedStudents = new(initialList.Where(s =>
                s.Id.ToString().Contains(searchText) ||
                s.Client.ChildFullname.ToLower().Contains(searchText) ||
                s.StudentSubscriptions.Any(ss => ss.Subscription.Name.ToLower().Contains(searchText)) ||
                s.StudentSubscriptions.Any(ss => ss.Course.Name.ToLower().Contains(searchText)) ||
                s.StudentSubscriptions.Any(ss => ss.SubscriptionStatus.Status.ToLower().Contains(searchText)) ||
                s.StudentSubscriptions.Any(ss => ss.StartDate.ToDateTime(TimeOnly.MinValue).ToString("dd.MM.yyyy").Contains(searchText)) ||
                s.StudentSubscriptions.Any(ss => ss.FinishDate.ToDateTime(TimeOnly.MinValue).ToString("dd.MM.yyyy").Contains(searchText))
                ));
        }
    }
}
