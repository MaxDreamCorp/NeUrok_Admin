using System.Windows;
using System.Windows.Controls;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для CoursesSelectorWindow.xaml
    /// </summary>
    public partial class CoursesSelectorWindow : Window
    {
        public event EventHandler<List<CourseDTO>>? CoursesSelected;

        public CoursesSelectorViewModel ViewModel { get; set; } = null!;

        public CoursesSelectorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void QuickSearchInp_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectionItem in ViewModel.FilteredCourses)
            {
                if (selectionItem.IsSelected)
                {
                    var it = ViewModel.AllCourses.Find(ci => ci.Course.Id == selectionItem.Course.Id);
                    if (it != null && !it.IsSelected)
                        it.IsSelected = true;
                }
            }

            CoursesSelected?.Invoke(this, ViewModel.AllCourses.Where(ci => ci.IsSelected)
                .Select(ci => ci.Course).ToList());
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
