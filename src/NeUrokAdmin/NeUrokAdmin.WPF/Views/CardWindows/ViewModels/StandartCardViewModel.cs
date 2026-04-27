using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.CardWindows.ViewModels
{
    public partial class StandartCardViewModel : ObservableObject
    {
        public event Action? Closed;

        public OperationType OperationType
        {
            get => _operationType;
            set
            {
                SetProperty(ref _operationType, value);
                IsDeletable = value == OperationType.Edit;
            }
        }

        private OperationType _operationType;

        [ObservableProperty]
        private BaseCardViewModel? _currentCard;

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private string _title = null!;

        [ObservableProperty]
        private string _headerText = null!;

        [RelayCommand]
        public void Back()
        {
            Closed?.Invoke();
        }

        [RelayCommand]
        public async Task Delete()
        {
            await CurrentCard.Delete();
        }

        [RelayCommand]
        public async Task Ok()
        {
            await CurrentCard.Ok();
        }
    }
}
