using System.Windows;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.UserControls;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.MainWindowViews;

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
            var vm = new ClientsViewViewModel();
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

            if (e.Type == Enums.TabType.Clients)
            {
                var vm = new ClientsViewViewModel();
                var clientsView = _navigationService.GetUserControl<ClientsView>();
                clientsView.ViewModel = vm;
                await clientsView.LoadData();
                MainConteiner.Content = clientsView;
            }
            if (e.Type == Enums.TabType.Students)
            {
                var vm = new StudentsViewViewModel();
                var studentsView = _navigationService.GetUserControl<StudentsView>();
                studentsView.ViewModel = vm;
                await studentsView.LoadData();
                MainConteiner.Content = studentsView;
            }
            else if (e.Type == Enums.TabType.Groups)
            {
                var vm = new GroupsViewViewModel();
                var groupsView = _navigationService.GetUserControl<GroupsView>();
                groupsView.ViewModel = vm;
                await groupsView.LoadData();
                MainConteiner.Content = groupsView;
            }
            else if (e.Type == Enums.TabType.Courses)
            {
                var vm = new CoursesViewViewModel();
                var coursesView = _navigationService.GetUserControl<CoursesView>();
                coursesView.ViewModel = vm;
                await coursesView.LoadData();
                MainConteiner.Content = coursesView;
            }
            else if (e.Type == Enums.TabType.Teachers)
            {
                var vm = new TeachersViewViewModel();
                var teachersView = _navigationService.GetUserControl<TeachersView>();
                teachersView.ViewModel = vm;
                await teachersView.LoadData();
                MainConteiner.Content = teachersView;
            }
        }
    }
}