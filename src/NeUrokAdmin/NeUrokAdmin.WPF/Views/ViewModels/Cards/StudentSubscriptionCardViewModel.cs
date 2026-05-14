using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class StudentSubscriptionCardViewModel : ObservableObject
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
                        HeaderText = "Создание абонемента ученика";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Абонемент ученика #\"{Id}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Абонемент ученика #\"{Id}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = true;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр";
                        break;
                    default:
                        break;
                }
            }
        }

        private OperationType _operationType;

        public List<SubscriptionStatusDTO> SubscriptionStatusesDTO
        {
            get => _subscriptionStatusesDTO;
            set
            {
                SetProperty(ref _subscriptionStatusesDTO, value);
                SubscriptionStatuses = value.Select(cs => cs.Status).ToList();
            }
        }

        [ObservableProperty]
        private List<string> _classesTypes = new();

        public List<ClassesTypeDTO> ClassesTypesDTO
        {
            get => _classesTypesDTO;
            set
            {
                SetProperty(ref _classesTypesDTO, value);
                ClassesTypes = value.Select(cs => cs.Type).ToList();
            }
        }
        private List<ClassesTypeDTO> _classesTypesDTO = new();

        [ObservableProperty]
        private bool _isDeletable;

        [ObservableProperty]
        private bool _isEditable;

        [ObservableProperty]
        private bool _isFilter;

        [ObservableProperty]
        private string _headerText = null!;

        [ObservableProperty]
        private List<string> _subscriptionStatuses = new List<string>();


        [ObservableProperty]
        private int? _id;

        [ObservableProperty]
        private string _studentFullname = string.Empty;

        [ObservableProperty]
        private string _classesType = string.Empty;

        [ObservableProperty]
        private decimal? _cost;

        [ObservableProperty]
        private int? _classesAmount;

        [ObservableProperty]
        private bool _isPaid;

        [ObservableProperty]
        private CourseDTO? _course;

        [ObservableProperty]
        private SubscriptionStatusDTO? _subscriptionStatus;

        [ObservableProperty]
        private string _subscriptionStatusStr = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _finishDate;

        private List<SubscriptionStatusDTO> _subscriptionStatusesDTO = new List<SubscriptionStatusDTO>();

        public StudentSubscriptionCardViewModel(OperationType operationType, string studentFullName, StudentSubscriptionDTO? studentSubscription = null)
        {
            _studentFullname = studentFullName;
            if (studentSubscription != null)
            {
                _id = studentSubscription.Id;
                _classesType = studentSubscription.ClassesType.Type;
                _cost = studentSubscription.Cost;
                _classesAmount = studentSubscription.ClassesAmount;
                _subscriptionStatus = studentSubscription.SubscriptionStatus;
                _subscriptionStatusStr = studentSubscription.SubscriptionStatus.Status;
                _isPaid = studentSubscription.IsPaid;
                _course = studentSubscription.Course;
                _startDate = new DateTime(studentSubscription.StartDate, TimeOnly.MinValue);
                _finishDate = new DateTime(studentSubscription.FinishDate, TimeOnly.MinValue);

            }
            OperationType = operationType;
        }

        public StudentSubscriptionDTO GetStudentSubscriptionDTO()
        {
            if (ClassesAmount == null || StartDate == null)
                throw new ArgumentNullException();

            if (!FinishDate.HasValue)
                FinishDate = StartDate.Value.AddDays(ClassesAmount.Value);

            return new StudentSubscriptionDTO(
                 Id ?? 0,
                 0,
                 ClassesTypesDTO.Find(ct => ct.Type == ClassesType) ?? throw new ArgumentNullException(nameof(ClassesType)),
                 Cost ?? throw new ArgumentNullException(nameof(Cost)),
                 ClassesAmount ?? throw new ArgumentNullException(nameof(ClassesAmount)),
                 IsPaid,
                 Course ?? throw new ArgumentNullException(nameof(Course)),
                 _subscriptionStatusesDTO.Find(ss => ss.Status == SubscriptionStatusStr) ?? throw new ArgumentNullException(nameof(SubscriptionStatusStr)),
                 StartDate.HasValue ? DateOnly.FromDateTime(StartDate.Value) : throw new ArgumentNullException(nameof(StartDate)),
                 FinishDate.HasValue ? DateOnly.FromDateTime(FinishDate.Value) : throw new ArgumentNullException(nameof(FinishDate))
                 );
        }
    }
}
