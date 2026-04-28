using System.Windows.Controls;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        private readonly NavigationService _navigationService;
        public ClientViewViewModel ViewModel { get; init; }

        public ClientsView(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            ViewModel = _navigationService.GetViewModel<ClientViewViewModel>();
            DataContext = ViewModel;
        }

        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.PrintAll();
        }
    }
}
