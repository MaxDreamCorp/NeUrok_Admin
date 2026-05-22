using System.Windows.Controls;
using System.Windows.Media;
using NeUrokAdmin.Domain.Enums;
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
            if (ViewModel.Group != null)
            {
                var studentSubscription = ViewModel.Student.StudentSubscriptions.FirstOrDefault(ss =>
                        ss.Course.Id == ViewModel.Group.Course.Id &&
                        (ss.ClassesType.Id == (int)ClassesTypeEnum.Group || ss.ClassesType.Id == (int)ClassesTypeEnum.Intensive) &&
                        ss.SubscriptionStatus.Id == (int)SubscriptionStatusEnum.Active);

                if (studentSubscription == null)
                {
                    _dialogService.ShowWarning("Не удалось найти подходящий абонемент у ученика");
                    return;
                }

                var vm = new AttendanceCardViewModel(ViewModel.Group, ViewModel.Student, ViewModel.Attendance, studentSubscription);
                var window = _navigationService.GetWindow<AttendanceCard>();
                window.ViewModel = vm;
                window.ShowDialog();
            }
        }
    }
}
