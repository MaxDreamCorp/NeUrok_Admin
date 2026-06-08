using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MediatR;
using NeUrokAdmin.Application.Features.GroupOperation.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.UserControls;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.Controls;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для JournalWindow.xaml
    /// </summary>
    public partial class JournalWindow : Window
    {
        public JournalWindowViewModel ViewModel { get; set; } = null!;

        private readonly NavigationService _navigationService;
        private readonly IMediator _mediator;

        public JournalWindow(NavigationService navigationService, IMediator mediator)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _mediator = mediator;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            BuildJournal();
        }

        private void BuildJournal()
        {
            JournalGrid.Children.Clear();
            JournalGrid.ColumnDefinitions.Clear();
            JournalGrid.RowDefinitions.Clear();
            
            for (int i = 0; i <= ViewModel.Dates.Count; i++)
            {
                ColumnDefinition cd = new ColumnDefinition()
                {
                    Width = GridLength.Auto
                };
                JournalGrid.ColumnDefinitions.Add(cd);
            }
            for (int i = 0; i <= ViewModel.StudentAttendances.Count + 3; i++)
            {
                RowDefinition rd = new()
                {
                    Height = GridLength.Auto
                };
                JournalGrid.RowDefinitions.Add(rd);
            }

            Border brd1 = new Border()
            {
                BorderThickness = new Thickness(0, 0, 1, 1),
                BorderBrush = new SolidColorBrush(Colors.Black)
            };
            JournalGrid.Children.Add(brd1);
            Grid.SetRowSpan(brd1, 3);

            int previousYear = 0;
            int previousMonth = 0;
            for (int i = 0; i < ViewModel.Dates.Count; i++)
            {
                var date = ViewModel.Dates[i];

                if (date.Year > previousYear)
                {
                    previousYear = date.Year;
                    Border yearBorder = new Border()
                    {
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Padding = new Thickness(2, 4, 2, 4),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };
                    TextBlock yearTextBlock = new TextBlock()
                    {
                        Text = date.ToString("yyyy"),
                        FontSize = 14,
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    yearBorder.Child = yearTextBlock;
                    JournalGrid.Children.Add(yearBorder);
                    Grid.SetColumn(yearBorder, i + 1);
                    Grid.SetRow(yearBorder, 0);
                    Grid.SetColumnSpan(yearBorder, ViewModel.Dates.Count(d => d.Year == previousYear));
                }

                if (date.Month > previousMonth)
                {
                    previousMonth = date.Month;
                    Border monthBorder = new Border()
                    {
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Padding = new Thickness(2, 4, 2, 4),
                        BorderBrush = new SolidColorBrush(Colors.Black)
                    };
                    TextBlock monthTextBlock = new TextBlock()
                    {
                        Text = date.ToString("MMMM"),
                        FontSize = 14,
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    monthBorder.Child = monthTextBlock;
                    JournalGrid.Children.Add(monthBorder);
                    Grid.SetColumn(monthBorder, i + 1);
                    Grid.SetRow(monthBorder, 1);
                    Grid.SetColumnSpan(monthBorder, ViewModel.Dates.Count(d => d.Month == previousMonth));
                }

                Border bd = new Border()
                {
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    MaxWidth = 60,
                    Padding = new Thickness(2, 4, 2, 4),
                };
                StackPanel sp = new StackPanel()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                TextBlock tbDate = new TextBlock()
                {
                    Text = date.ToString("dd.MM"),
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                TextBlock tbTime = new TextBlock()
                {
                    Text = date.ToString("HH:mm"),
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center,
                };
                sp.Children.Add(tbDate);
                sp.Children.Add(tbTime);
                bd.Child = sp;
                JournalGrid.Children.Add(bd);
                Grid.SetRow(bd, 2);
                Grid.SetColumn(bd, i + 1);
            }

            for (int i = 0; i < ViewModel.StudentAttendances.Count; i++)
            {
                var item = new StudentAttendancesDTO(
                    ViewModel.StudentAttendances[i].Student,
                    ViewModel.StudentAttendances[i].StudentSubscription,
                    ViewModel.StudentAttendances[i].Group,
                    ViewModel.StudentAttendances[i].Attendances.OrderBy(a => a.Datetime).ToList());

                Border bd = new Border()
                {
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                TextBlock tb = new TextBlock()
                {
                    Text = item.Student.Client.ChildFullname,
                    Padding = new Thickness(3),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                };
                bd.Child = tb;
                JournalGrid.Children.Add(bd);
                Grid.SetRow(bd, i + 3);

                for (global::System.Int32 j = 0; j < item.Attendances.Count; j++)
                {
                    var attendance = item.Attendances[j];
                    var vm = new JournalCellViewModel(attendance, item.Student, item.StudentSubscription, item.Group);
                    var cell = _navigationService.GetUserControl<JournalCell>();
                    cell.ViewModel = vm;
                    cell.AttendancesNeedsToUpdate += Cell_AttendancesNeedsToUpdate;
                    cell.Load();
                    JournalGrid.Children.Add(cell);
                    Grid.SetRow(cell, i + 3);
                    Grid.SetColumn(cell, j + 1);
                }
            }
        }

        private async void Cell_AttendancesNeedsToUpdate()
        {
            await UpdateAttendances();
            BuildJournal();
        }

        private async Task UpdateAttendances()
        {
            var group = await _mediator.Send(new GetGroupByIdQuery(ViewModel.Group.Id));
            var attendances = await _mediator.Send(new GetGroupStudentsAttendancesCommand(ViewModel.Group.Id));

            ViewModel = new(group, attendances);
        }
    }
}
