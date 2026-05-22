using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Queries;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для AttendanceCard.xaml
    /// </summary>
    public partial class AttendanceCard : Window
    {
        public AttendanceCardViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;

        public AttendanceCard(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var statuses = await _mediator.Send(new GetAttendanceStatusesQuery());
            ViewModel.AttendanceStatusesDTO = statuses;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
