using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NeUrokAdmin.WPF.Views.Elements.ViewModels
{
    public abstract partial class BaseDataViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _quickSearchText = null!;

        [ObservableProperty]
        private bool _isBusy;

        [RelayCommand]
        public abstract Task Create();

        [RelayCommand]
        public abstract Task QuickSearch();

        [RelayCommand]
        public abstract Task Filter();

        [RelayCommand]
        public abstract Task ClearSearchings();
    }
}
