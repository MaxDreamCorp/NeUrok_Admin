using System.Windows.Controls;
using System.Windows.Media;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;
using NeUrokAdmin.WPF.Views.ViewModels.Controls;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для JournalCell.xaml
    /// </summary>
    public partial class JournalCell : UserControl
    {
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private JournalCellViewModel _viewModel = null!;
        public JournalCellViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;

                if (value.Attendance.Mark == "-")
                    MC.Background = new SolidColorBrush(Colors.Coral);
                else if (value.Attendance.Mark == "у")
                    MC.Background = new SolidColorBrush(Colors.LightBlue);
                else
                    MC.Background = new SolidColorBrush(Colors.White);
            }
        }

        public JournalCell(NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public void Load()
        {
            DataContext = ViewModel;
        }

        private void MC_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vm = new AttendanceCardViewModel(ViewModel.Group, ViewModel.Student, ViewModel.Attendance, ViewModel.StudentSubscription);
            var window = _navigationService.GetWindow<AttendanceCard>();
            window.ViewModel = vm;
            window.AttendanceChanged += Window_AttendanceChanged;
            window.ShowDialog();
        }

        private void Window_AttendanceChanged(object? sender, Domain.DTOs.AttendanceDTO e)
        {
            ViewModel = new JournalCellViewModel(e, ViewModel.Student, ViewModel.StudentSubscription, ViewModel.Group);
            DataContext = ViewModel;
        }
    }
}
