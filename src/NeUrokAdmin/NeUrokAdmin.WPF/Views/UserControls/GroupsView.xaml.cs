using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для GroupsView.xaml
    /// </summary>
    public partial class GroupsView : UserControl
    {
        public GroupsViewViewModel ViewModel { get; set; } = null!;

        public GroupsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

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

        public async Task LoadData()
        {
            DataContext = ViewModel;
            await Clear();
        }

        private async Task Clear()
        {

        }
    }
}
