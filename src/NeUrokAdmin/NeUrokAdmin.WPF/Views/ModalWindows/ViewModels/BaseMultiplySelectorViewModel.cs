using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NeUrokAdmin.WPF.Views.ModalWindows.ViewModels
{
    public abstract partial class BaseMultiplySelectorViewModel : ObservableObject
    {



        public abstract IEnumerable<object> AllItems { get; }

        [RelayCommand] public abstract void Select();
        [RelayCommand] public abstract void Back();
        [RelayCommand] public abstract void QuickSearch();
    }
}
