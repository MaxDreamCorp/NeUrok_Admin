using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NeUrokAdmin.WPF.Views.ViewModels.Controls;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для JournalCell.xaml
    /// </summary>
    public partial class JournalCell : UserControl
    {
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

        public JournalCell()
        {
            InitializeComponent();
        }

        public void Load()
        {
            DataContext = ViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
