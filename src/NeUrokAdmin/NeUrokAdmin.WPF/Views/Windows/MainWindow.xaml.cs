using System.Windows;
using NeUrokAdmin.WPF.Services;

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
        }
    }
}