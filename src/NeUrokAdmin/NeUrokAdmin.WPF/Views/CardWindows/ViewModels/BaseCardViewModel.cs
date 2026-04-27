using CommunityToolkit.Mvvm.ComponentModel;

namespace NeUrokAdmin.WPF.Views.CardWindows.ViewModels
{
    public abstract partial class BaseCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isFilter;

        [ObservableProperty]
        private bool _isEditable;

        public abstract Task Delete();

        public abstract Task Ok();
    }
}
