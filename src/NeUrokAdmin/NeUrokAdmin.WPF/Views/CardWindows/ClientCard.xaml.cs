using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для ClientCard.xaml
    /// </summary>
    public partial class ClientCard : Window
    {
        private readonly IMediator _mediator;
        public ClientCardViewModel ViewModel { get; set; } = null!;

        public ClientCard(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var qry = new GetAllClientsStatusesQuery();
            var statuses = await _mediator.Send(qry);

            ViewModel.ClientStatuses = statuses.Select(x => x.Status).ToList();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
