using CommunityToolkit.Mvvm.ComponentModel;

namespace NeUrokAdmin.WPF.Views.Elements.ViewModels
{
    public partial class SelectableItem<T> : ObservableObject
    {
        public T Data { get; }

        [ObservableProperty]
        private bool _isSelected;

        public SelectableItem(T data, bool isSelected = false)
        {
            Data = data;
            _isSelected = isSelected;
        }
    }
}
