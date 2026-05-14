using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.StudentOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors.SelectorItems;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для StudentsSelectorWindow.xaml
    /// </summary>
    public partial class StudentsSelectorWindow : Window
    {
        public StudentsSelectorViewModel ViewModel { get; set; } = null!;
        public event EventHandler<List<StudentDTO>>? StudentsSelected;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public StudentsSelectorWindow(IMediator mediator, NavigationService navigationService)
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
            CopyFilterSelectedToAllStudents();

            var searchText = ViewModel.QuickSearchText.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                ViewModel.FilteredStudents = ViewModel.AllStudents;
                return;
            }


            ViewModel.FilteredStudents = ViewModel.AllStudents
               .Where(s =>
                s.Student.Id.ToString().Contains(searchText) ||
                s.Student.Client.ChildFullname.ToLower().Contains(searchText) ||
                s.Student.StudentSubscriptions.Any(ss => ss.ClassesType.Type.ToLower().Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.Cost.ToString().Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.ClassesAmount.ToString().Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.Course.Name.ToLower().Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.SubscriptionStatus.Status.ToLower().Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.StartDate.ToDateTime(TimeOnly.MinValue).ToString("dd.MM.yyyy").Contains(searchText)) ||
                s.Student.StudentSubscriptions.Any(ss => ss.FinishDate.ToDateTime(TimeOnly.MinValue).ToString("dd.MM.yyyy").Contains(searchText)))
               .ToList();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new StudentCardViewModel(Enums.OperationType.Create);
            var card = _navigationService.GetWindow<StudentCard>();
            card.ViewModel = vm;
            card.ShowDialog();
            if (card.DialogResult == true)
                await RefreeshList();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            CopyFilterSelectedToAllStudents();

            StudentsSelected?.Invoke(this, ViewModel.AllStudents.Where(ci => ci.IsSelected)
                .Select(ci => ci.Student).ToList());
            Close();
        }

        private async void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.Content is StudentSelectorItemViewModel vm)
            {
                var dto = vm.Student;

                var cardVm = new StudentCardViewModel(Enums.OperationType.Edit, dto);

                var editCard = _navigationService.GetWindow<StudentCard>();
                editCard.ViewModel = cardVm;
                editCard.ShowDialog();
                if (editCard.DialogResult == true)
                    await RefreeshList();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSingleton)
                return;

            if (sender is CheckBox checkBox && checkBox.DataContext is StudentSelectorItemViewModel vm)
            {
                var it = ViewModel.AllStudents.Find(ci => ci.Student.Id == vm.Student.Id);
                if (it != null)
                {
                    foreach (var selItem in ViewModel.AllStudents)
                    {
                        if (selItem.Student.Id != it.Student.Id)
                            selItem.IsSelected = false;
                    }
                }
            }
        }

        private async Task RefreeshList()
        {
            var selectedStudents = ViewModel.AllStudents.Where(ci => ci.IsSelected).Select(ci => ci.Student).ToList();

            var allStudents = await _mediator.Send(new GetAllStudentsQuery());
            var newVM = new StudentsSelectorViewModel(allStudents, selectedStudents);
            ViewModel = newVM;
            DataContext = ViewModel;
        }

        private void CopyFilterSelectedToAllStudents()
        {
            foreach (var selectionItem in ViewModel.FilteredStudents)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllStudents.Find(ci => ci.Student.Id == selectionItem.Student.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }
        }
    }
}
