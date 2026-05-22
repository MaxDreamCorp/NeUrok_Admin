using System.Windows;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для AttendanceCard.xaml
    /// </summary>
    public partial class AttendanceCard : Window
    {
        public AttendanceCardViewModel ViewModel { get; set; } = null!;

        public AttendanceCard()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
