using System.Windows;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для JournalWindow.xaml
    /// </summary>
    public partial class JournalWindow : Window
    {
        public JournalWindowViewModel ViewModel { get; set; } = null!;

        private readonly NavigationService _navigationService;

        public JournalWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }
    }
}
