using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.CardWindows;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для GroupsView.xaml
    /// </summary>
    public partial class GroupsView : UserControl
    {
        public GroupsViewViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public GroupsView(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var vm = new GroupCardViewModel(Enums.OperationType.Create);
            var groupCard = _navigationService.GetWindow<GroupCard>();
            groupCard.ViewModel = vm;
            groupCard.ShowDialog();
            await Clear();
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

        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is GroupDTO group)
            {
                var cardVM = new GroupCardViewModel(Enums.OperationType.Edit, group);
                var card = _navigationService.GetWindow<GroupCard>();
                card.ViewModel = cardVM;
                card.ShowDialog();
                if (card.DialogResult == true)
                {
                    await PrintAll();
                    //await Refilter();
                    QuickSearch();
                }
            }
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

        private void QuickSearch()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.QuickSearchText))
            {
                ResetDisplayedGroupsAfterSearching();
                return;
            }

            var lowerSearchText = ViewModel.QuickSearchText.ToLower();
            ViewModel.DisplayedGroups = new(ViewModel.AllGroups.Where(g =>
                g.Id.ToString().Contains(lowerSearchText) ||
                g.Name.ToLower().Contains(lowerSearchText) ||
                g.Course.Name.ToLower().Contains(lowerSearchText) ||
                g.Teacher.Fullname.ToLower().Contains(lowerSearchText) ||
                g.GroupStatus.Status.ToLower().Contains(lowerSearchText) ||
                g.WeekDays.ToLower().Contains(lowerSearchText) ||
                g.Time.ToString("HH:mm").Contains(lowerSearchText))
                .ToList());
        }

        private async Task PrintAll()
        {
            var qry = new GetAllGroupsQuery();
            ViewModel.AllGroups = await _mediator.Send(qry);
            ViewModel.DisplayedGroups = new(ViewModel.AllGroups);
        }

        private void ResetDisplayedGroupsAfterSearching()
        {
            ViewModel.DisplayedGroups = ViewModel.FilteredGroups == null ?
                new(ViewModel.AllGroups) :
                new(ViewModel.FilteredGroups);
        }

    }
}
