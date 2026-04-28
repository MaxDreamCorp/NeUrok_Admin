using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels
{
    public partial class ClientCardViewModel : ObservableObject
    {
        public OperationType OperationType
        {
            get => _operationType;
            set
            {
                SetProperty(ref _operationType, value);
                IsDeletable = value == OperationType.Edit;
                IsEditable = value != OperationType.Read;
            }
        }

        private OperationType _operationType;

        [ObservableProperty]
        private ClientDTO? _client;

        [ObservableProperty]
        private List<string> _clientStatuses = new();

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private string _headerText = null!;

        public ClientCardViewModel(OperationType operationType, string headerText, ClientDTO? clientDTO = null)
        {
            OperationType = operationType;
            _headerText = headerText;
            _client = clientDTO;
        }

    }
}
