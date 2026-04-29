using System.Windows.Controls;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
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

        private async void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ClientDTO client)
            {
                var cardVM = new ClientCardViewModel(Enums.OperationType.Edit, client);
                var card = _navigationService.GetWindow<ClientCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                    await ViewModel.PrintAll();
            }
        }
    }
}
