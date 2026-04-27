using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModels
{
    public partial class MultiplySelectorViewModel : ObservableObject
    {
        public event Action? Closed;

        [ObservableProperty]
        private string _title = "Выбор элементов";

        [ObservableProperty]
        private string _quickSearchText = string.Empty;

        [ObservableProperty]
        private BaseMultiplySelectorViewModel _currentSelector;

        [RelayCommand]
        public void Select()
        {

        }

        [RelayCommand]
        public void Back()
        {
            Closed?.Invoke();
        }

        [RelayCommand]
        public void QuickSearch()
        {

        }
    }
}
