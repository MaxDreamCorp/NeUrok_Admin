using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class SubscriptionCardViewModel : ObservableObject
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
                        HeaderText = "Создание абонемента";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Абонемент \"{Name}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Абонемент \"{Name}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = true;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр абонементов";
                        break;
                    default:
                        break;
                }
            }
        }

        private OperationType _operationType;

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
        private int? _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _classesType = string.Empty;

        [ObservableProperty]
        private decimal? _cost;

        [ObservableProperty]
        private int? _classesAmount;

        public SubscriptionCardViewModel(OperationType operationType, SubscriptionDTO? subscriptionDTO = null)
        {
            if (subscriptionDTO != null)
            {
                _id = subscriptionDTO.Id;
                _name = subscriptionDTO.Name;
                _classesType = subscriptionDTO.ClassesType.Type;
                _cost = subscriptionDTO.Cost;
                _classesAmount = subscriptionDTO.ClassesAmount;
            }
            OperationType = operationType;
        }
    }
}
