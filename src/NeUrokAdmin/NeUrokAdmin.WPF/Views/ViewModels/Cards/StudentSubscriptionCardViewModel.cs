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
        private SubscriptionDTO? _subscription;

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

        public StudentSubscriptionCardViewModel(OperationType operationType, string studentFullName, StudentSubscriptionDTO? studentSubscription = null)
        {
            _studentFullname = studentFullName;
            if (studentSubscription != null)
            {
                _id = studentSubscription.Id;
                _subscription = studentSubscription.Subscription;
                _subscriptionStatus = studentSubscription.SubscriptionStatus;
                _subscriptionStatusStr = studentSubscription.SubscriptionStatus.Status;
                _isPaid = studentSubscription.IsPaid;
                _course = studentSubscription.Course;
                _startDate = new DateTime(studentSubscription.StartDate, TimeOnly.MinValue);
                _finishDate = new DateTime(studentSubscription.FinishDate, TimeOnly.MinValue);

            }
            OperationType = operationType;
        }
    }
}
