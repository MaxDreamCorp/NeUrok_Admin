using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;
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
                        IsFilter = true;
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

        public ClientSearchDTO? SearchDTO { get; set; }

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
        private List<int> _ids;

        [ObservableProperty]
        private List<int> _days = Enumerable.Range(1, 31).ToList();

        [ObservableProperty]
        private List<int> _months = Enumerable.Range(1, 12).ToList();

        [ObservableProperty]
        private List<int> _years = Enumerable.Range(2000, DateTime.Now.Year - 2000).Reverse().ToList();

        [ObservableProperty]
        private List<int> _grades = Enumerable.Range(-6, 17).ToList();


        [ObservableProperty]
        private int? _id;

        [ObservableProperty]
        private string _childFullname = null!;

        [ObservableProperty]
        private DateTime? _birthDate;

        [ObservableProperty]
        private DateTime? _registrationDate;

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
        private int? _idFrom;

        [ObservableProperty]
        private int? _idTo;

        [ObservableProperty]
        private DateTime? _birthDateFrom;

        [ObservableProperty]
        private DateTime? _birthDateTo;

        [ObservableProperty]
        private int? _birthDateDay;

        [ObservableProperty]
        private int? _birthDateMonth;

        [ObservableProperty]
        private int? _birthDateYear;

        [ObservableProperty]
        private DateTime? _registrationDateFrom;

        [ObservableProperty]
        private DateTime? _registrationDateTo;

        [ObservableProperty]
        private int? _registrationDateDay;

        [ObservableProperty]
        private int? _registrationDateMonth;

        [ObservableProperty]
        private int? _registrationDateYear;


        [ObservableProperty]
        private int? _gradeFrom;

        [ObservableProperty]
        private int? _gradeTo;

        [ObservableProperty]
        private ObservableCollection<CourseDTO> _wishedCourses = new();

        [ObservableProperty]
        private string _wishedCoursesDisplay = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ClientStatusDTO> _searchingStatuses = new();

        [ObservableProperty]
        private string _searchingStatusesDisplay = string.Empty;

        private List<ClientStatusDTO> _clientStatusesDTO = new();

        public ClientCardViewModel(OperationType operationType, ClientDTO? clientDTO = null, int? maxId = null)
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

            if (SearchDTO != null)
            {
                Id = SearchDTO.Id;
                ChildFullname = SearchDTO.ChildFullname ?? string.Empty;
                ParentName = SearchDTO.ParentName ?? string.Empty;
                Phone = SearchDTO.Phone ?? string.Empty;
                AdditionalPhone = SearchDTO.AdditionalPhone ?? string.Empty;
                Status = SearchDTO.Status ?? string.Empty;
                Notes = SearchDTO.Notes ?? string.Empty;

                IdFrom = SearchDTO.IdFrom;
                IdTo = SearchDTO.IdTo;
                Grade = SearchDTO.Grade;
                GradeFrom = SearchDTO.GradeFrom;
                GradeTo = SearchDTO.GradeTo;

                BirthDate = SearchDTO.BirthDate.HasValue
                    ? SearchDTO.BirthDate.Value.ToDateTime(TimeOnly.MinValue)
                    : null;
                BirthDateFrom = SearchDTO.BirthDateFrom.HasValue
                    ? SearchDTO.BirthDateFrom.Value.ToDateTime(TimeOnly.MinValue)
                    : null;
                BirthDateTo = SearchDTO.BirthDateTo.HasValue
                    ? SearchDTO.BirthDateTo.Value.ToDateTime(TimeOnly.MinValue)
                    : null;

                BirthDateDay = SearchDTO.BirthDateDay;
                BirthDateMonth = SearchDTO.BirthDateMonth;
                BirthDateYear = SearchDTO.BirthDateYear;

                RegistrationDate = SearchDTO.RegistrationDate.HasValue
                    ? SearchDTO.RegistrationDate.Value.ToDateTime(TimeOnly.MinValue)
                    : DateTime.Now;

                RegistrationDateFrom = SearchDTO.RegistrationDateFrom.HasValue
                    ? SearchDTO.RegistrationDateFrom.Value.ToDateTime(TimeOnly.MinValue)
                    : null;
                RegistrationDateTo = SearchDTO.RegistrationDateTo.HasValue
                    ? SearchDTO.RegistrationDateTo.Value.ToDateTime(TimeOnly.MinValue)
                    : null;

                RegistrationDateDay = SearchDTO.RegistrationDateDay;
                RegistrationDateMonth = SearchDTO.RegistrationDateMonth;
                RegistrationDateYear = SearchDTO.RegistrationDateYear;

                WishedCourses = new ObservableCollection<CourseDTO>(SearchDTO.WishedCourses ?? new List<CourseDTO>());
                SearchingStatuses = new ObservableCollection<ClientStatusDTO>(SearchDTO.Statuses ?? new List<ClientStatusDTO>());
                WishedCoursesDisplay = string.Join(", ", WishedCourses.Select(c => c.Name));
                SearchingStatusesDisplay = string.Join(", ", SearchingStatuses.Select(s => s.Status));
            }
            _ids = Enumerable.Range(1, maxId ?? 100).ToList();
            if (operationType == OperationType.Create)
                _registrationDate = DateTime.Now;
            OperationType = operationType;
        }

        public ClientDTO GetClientDTO()
        {
            return new ClientDTO(
                Id: Id ?? int.MinValue,
                ChildFullname: ChildFullname,
                BirthDate: BirthDate.HasValue ? DateOnly.FromDateTime(BirthDate.Value) : null,
                RegistrationDate: RegistrationDate.HasValue ? DateOnly.FromDateTime(RegistrationDate.Value) : throw new InvalidOperationException("Дата регистрации не указана"),
                Grade: Grade,
                Status: ClientStatusesDTO.Find(cs => cs.Status == Status) ?? throw new InvalidOperationException("Статус не выбран  "),
                ParentName: ParentName,
                Phone: Phone,
                WishedCourses: WishedCourses.ToList(),
                Notes: Notes,
                AdditionalPhones: AdditionalPhone);
        }

        public ClientSearchDTO GetSearchDTO()
        {
            SearchDTO = new ClientSearchDTO(
                Id: Id,
                ChildFullname: string.IsNullOrWhiteSpace(ChildFullname) ? null : ChildFullname,
                ParentName: string.IsNullOrWhiteSpace(ParentName) ? null : ParentName,
                Phone: string.IsNullOrWhiteSpace(Phone) ? null : Phone,
                AdditionalPhone: string.IsNullOrWhiteSpace(AdditionalPhone) ? null : AdditionalPhone,
                Status: string.IsNullOrWhiteSpace(Status) ? null : Status,
                Notes: string.IsNullOrWhiteSpace(Notes) ? null : Notes,
                IdFrom: IdFrom,
                IdTo: IdTo,
                Grade: Grade,
                GradeFrom: GradeFrom,
                GradeTo: GradeTo,
                BirthDate: BirthDate.HasValue ? DateOnly.FromDateTime(BirthDate.Value) : null,
                BirthDateFrom: BirthDateFrom.HasValue ? DateOnly.FromDateTime(BirthDateFrom.Value) : null,
                BirthDateTo: BirthDateTo.HasValue ? DateOnly.FromDateTime(BirthDateTo.Value) : null,
                BirthDateDay: BirthDateDay,
                BirthDateMonth: BirthDateMonth,
                BirthDateYear: BirthDateYear,
                RegistrationDate: RegistrationDate.HasValue ? DateOnly.FromDateTime(RegistrationDate.Value) : null,
                RegistrationDateFrom: RegistrationDateFrom.HasValue ? DateOnly.FromDateTime(RegistrationDateFrom.Value) : null,
                RegistrationDateTo: RegistrationDateTo.HasValue ? DateOnly.FromDateTime(RegistrationDateTo.Value) : null,
                RegistrationDateDay: RegistrationDateDay,
                RegistrationDateMonth: RegistrationDateMonth,
                RegistrationDateYear: RegistrationDateYear,
                WishedCourseIds: WishedCourses.Select(c => c.Id).ToList(),
                StatusIds: SearchingStatuses.Select(s => s.Id).ToList(),
                WishedCourses: WishedCourses.ToList(),
                Statuses: SearchingStatuses.ToList()
            );
            return SearchDTO;
        }
    }
}
