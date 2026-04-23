using System.Windows;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Windows.ViewModels;

namespace NeUrokAdmin.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;


        public MainWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;

            var vm = _navigationService.GetViewModel<MainWindowViewModel>();
            DataContext = vm;
        }
    }
}