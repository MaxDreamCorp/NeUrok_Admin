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

                switch (value)
                {
                    case OperationType.Create:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Создание клиента";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Клиент \"{ChildFullname}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Клиент \"{ChildFullname}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр клиентов";
                        break;
                    default:
                        break;
                }
            }
        }

        private OperationType _operationType;

        [ObservableProperty]
        private List<string> _clientStatuses = new();

        public List<ClientStatusDTO> ClientStatusesDTO
        {
            get => _clientStatusesDTO;
            set
            {
                SetProperty(ref _clientStatusesDTO, value);
                ClientStatuses = value.Select(cs => cs.Status).ToList();
            }
        }

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private bool _isFilter;

        [ObservableProperty]
        private string _headerText = null!;


        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _childFullname = null!;

        [ObservableProperty]
        private DateTime? _birthDate;

        [ObservableProperty]
        private DateTime _registrationDate;

        [ObservableProperty]
        private int? _grade;

        [ObservableProperty]
        private string _status = string.Empty;

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
        private string _wishedCoursesDisplay = string.Empty;

        private List<ClientStatusDTO> _clientStatusesDTO = new();

        public ClientCardViewModel(OperationType operationType, ClientDTO? clientDTO = null)
        {
            if (clientDTO != null)
            {
                _id = clientDTO.Id;
                _childFullname = clientDTO.ChildFullname;
                _birthDate = clientDTO.BirthDate.HasValue ? new DateTime(clientDTO.BirthDate.Value, TimeOnly.MinValue) : null;
                _registrationDate = new DateTime(clientDTO.RegistrationDate, TimeOnly.MinValue);
                _grade = clientDTO.Grade;
                _status = clientDTO.Status.Status;
                _parentName = clientDTO.ParentName;
                _phone = clientDTO.Phone;
                _additionalPhone = clientDTO.AdditionalPhones ?? string.Empty;
                _notes = clientDTO.Notes;
                _wishedCourses = new ObservableCollection<CourseDTO>(clientDTO.WishedCourses ?? new List<CourseDTO>());
                _wishedCoursesDisplay = string.Join(", ", _wishedCourses.Select(c => c.Name));
            }

            OperationType = operationType;
            _registrationDate = DateTime.Now;
        }

        public ClientDTO GetClientDTO()
        {
            return new ClientDTO(
                Id: Id,
                ChildFullname: ChildFullname,
                BirthDate: BirthDate.HasValue ? DateOnly.FromDateTime(BirthDate.Value) : null,
                RegistrationDate: DateOnly.FromDateTime(RegistrationDate),
                Grade: Grade,
                Status: ClientStatusesDTO.Find(cs => cs.Status == Status) ?? throw new InvalidOperationException("Статус не выбран  "),
                ParentName: ParentName,
                Phone: Phone,
                WishedCourses: WishedCourses.ToList(),
                Notes: Notes,
                AdditionalPhones: AdditionalPhone);
        }
    }
}
