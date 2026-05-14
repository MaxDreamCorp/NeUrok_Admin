using System.Windows;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для ClientStatusesSelectorWindow.xaml
    /// </summary>
    public partial class ClientStatusesSelectorWindow : Window
    {
        public ClientStatusesSelectorViewModel ViewModel { get; set; } = null!;
        public event EventHandler<List<ClientStatusDTO>>? ClientStatusesSelected;

        public ClientStatusesSelectorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientStatusesSelected?.Invoke(this, ViewModel.AllClientStatuses.Where(cs => cs.IsSelected)
                .Select(cs => cs.ClientStatus).ToList());
            Close();
        }
    }
}
