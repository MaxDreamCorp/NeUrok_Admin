using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для GroupsView.xaml
    /// </summary>
    public partial class GroupsView : UserControl
    {
        public GroupsViewViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;

        public GroupsView(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
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
            ViewModel.FilteredGroups = null;
            ViewModel.IsFiltering = false;
            ViewModel.QuickSearchText = string.Empty;
            await PrintAll();
        }

        private async Task PrintAll()
        {
            var qry = new GetAllGroupsQuery();
            ViewModel.AllGroups = await _mediator.Send(qry);
            ViewModel.DisplayedGroups = new(ViewModel.AllGroups);
        }
    }
}
