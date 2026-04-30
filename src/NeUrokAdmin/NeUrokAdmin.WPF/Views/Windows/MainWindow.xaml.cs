using System.Windows;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.UserControls;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            _viewModel.SideMenuItemClicked += _viewModel_SideMenuItemClicked;
            DataContext = _viewModel;

            _navigationService = navigationService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = new ClientViewViewModel();
            var clientsView = _navigationService.GetUserControl<ClientsView>();
            clientsView.ViewModel = vm;
            await clientsView.LoadData();
            MainConteiner.Content = clientsView;
        }

        private async void _viewModel_SideMenuItemClicked(object? sender, SideMenuItemViewModel e)
        {

            foreach (var item in _viewModel.SideMenuItems)
                item.IsSelected = false;
            e.IsSelected = true;

            switch (e.Type)
            {
                case Enums.TabType.Clients:
                    var vm = new ClientViewViewModel();
                    var clientsView = _navigationService.GetUserControl<ClientsView>();
                    clientsView.ViewModel = vm;
                    await clientsView.LoadData();
                    MainConteiner.Content = clientsView;
                    break;
                case Enums.TabType.Students:
                    break;
                case Enums.TabType.Groups:
                    break;
                case Enums.TabType.Subscriptions:
                    break;
                default:
                    break;
            }
        }


    }
}