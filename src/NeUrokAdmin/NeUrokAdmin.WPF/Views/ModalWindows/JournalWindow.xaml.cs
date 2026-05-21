using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.ViewModels;

namespace NeUrokAdmin.WPF.Views.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для JournalWindow.xaml
    /// </summary>
    public partial class JournalWindow : Window
    {
        public JournalWindowViewModel ViewModel { get; set; } = null!;

        private readonly NavigationService _navigationService;

        public JournalWindow(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
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
                var item = ViewModel.StudentAttendances[i];

                Border bd = new Border()
                {
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    BorderBrush = new SolidColorBrush(Colors.Black)
                };
                TextBlock tb = new TextBlock()
                {
                    Text = item.Student.Client.ChildFullname,
                    Padding = new Thickness(2, 4, 2, 4),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                };
                bd.Child = tb;
                JournalGrid.Children.Add(bd);
                Grid.SetRow(bd, i + 3);
            }
        }
    }
}
