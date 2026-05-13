using System.Windows;
using MediatR;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentCard.xaml
    /// </summary>
    public partial class StudentCard : Window
    {
        public StudentCardViewModel ViewModel { get; set; } = null!;

        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;

        public StudentCard(IMediator mediator, NavigationService navigationService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectClientBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSubscriptionBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
