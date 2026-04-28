using System.Collections.ObjectModel;
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
        private string _childFullname = null!;

        [ObservableProperty]
        private DateOnly? _birthDate;

        [ObservableProperty]
        private int _grade;

        [ObservableProperty]
        private ClientStatusDTO? _status;

        [ObservableProperty]
        private string _parentName = null!;

        [ObservableProperty]
        private string _phone = null!;

        [ObservableProperty]
        private string _additionalPhone = null!;

        [ObservableProperty]
        private string? _notes;

        [ObservableProperty]
        private ObservableCollection<CourseDTO> _wishedCourses = new();

        [ObservableProperty]
        private string _wishedCoursesDisplay;

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


            _wishedCoursesDisplay = _wishedCourses != null ?
                string.Join(", ", _wishedCourses.Select(c => c.Name))
                : "";
        }

    }
}
