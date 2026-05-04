using System.Windows;
using System.Windows.Controls;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для GroupCard.xaml
    /// </summary>
    public partial class GroupCard : Window
    {
        public GroupCardViewModel ViewModel { get; set; } = null!;

        public GroupCard()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectCourseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectTeacherBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Calendar calendar)
            {
                ViewModel.SelectedDates = calendar.SelectedDates
                    .Order().ToList();
                ViewModel.ClassesDates = string.Join(", ", ViewModel.SelectedDates
                    .Order().Select(d =>
                    $"Занятие {ViewModel.SelectedDates.IndexOf(d) + 1}: {d.ToShortDateString()}"));
            }
        }

        private void AcceptDatesBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.WeekDays = string.Join(", ", ViewModel.SelectedDates.
                Order().Select(d => d.ToString("ddd")).Distinct());
        }
    }
}
