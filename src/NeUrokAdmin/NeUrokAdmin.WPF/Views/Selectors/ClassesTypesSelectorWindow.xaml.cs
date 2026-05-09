using System.Windows;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.Selectors
{
    /// <summary>
    /// Логика взаимодействия для ClassesTypesSelectorWindow.xaml
    /// </summary>
    public partial class ClassesTypesSelectorWindow : Window
    {
        public ClassesTypesSelectorViewModel ViewModel { get; set; } = null!;
        public event EventHandler<List<ClassesTypeDTO>>? ClassesTypesSelected;

        public ClassesTypesSelectorWindow()
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

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            ClassesTypesSelected?.Invoke(this, ViewModel.AllClassesTypes.Where(ct => ct.IsSelected)
                .Select(ct => ct.ClassesType).ToList());
            Close();
        }
    }
}
