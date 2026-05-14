using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class StudentCardViewModel : ObservableObject
    {
        public OperationType OperationType
        {
            get => _operationType;
            set
            {
                SetProperty(ref _operationType, value);

                switch (value)
                {
                    case OperationType.Create:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Создание ученика";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Ученик \"{Client?.ChildFullname}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Ученик \"{Client?.ChildFullname}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = true;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр учеников";
                        break;
                    default:
                        break;
                }
            }
        }

        private OperationType _operationType;

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private bool _isFilter;

        [ObservableProperty]
        private string _headerText = null!;


        [ObservableProperty]
        private int? _id;

        [ObservableProperty]
        private ClientDTO? _client;

        [ObservableProperty]
        private ObservableCollection<StudentSubscriptionDTO> _subscriptions = new();


        public StudentCardViewModel(OperationType operationType, StudentDTO? studentDTO = null)
        {
            if (studentDTO != null)
            {
                _id = studentDTO.Id;
                _client = studentDTO.Client;
                _subscriptions = new(studentDTO.StudentSubscriptions);
            }
            OperationType = operationType;
        }

        public StudentDTO GetStudentDTO()
        {
            return new StudentDTO(
                Id.HasValue ? Id.Value : 0,
                Client ?? throw new InvalidOperationException("Клиент не должен быть пустым"),
                Subscriptions.ToList());
        }
    }
}
