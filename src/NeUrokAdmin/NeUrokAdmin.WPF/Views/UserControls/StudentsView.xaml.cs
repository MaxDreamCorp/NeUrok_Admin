using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
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
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private async Task PrintAll()
        {
            var qry = new GetAllStudentsQuery();
            ViewModel.AllStudents = await _mediator.Send(qry);
            ViewModel.DisplayedStudents = new(ViewModel.AllStudents);
        }
    }
}
