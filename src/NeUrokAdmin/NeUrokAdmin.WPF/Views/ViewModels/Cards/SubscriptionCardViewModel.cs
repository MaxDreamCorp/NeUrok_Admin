using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;
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

        public SubscriptionSearchDTO? SearchDTO { get; set; }

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
        private List<int> _ids;

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


        [ObservableProperty]
        private int? _idFrom;

        [ObservableProperty]
        private int? _idTo;

        [ObservableProperty]
        private ObservableCollection<ClassesTypeDTO> _searchingClassesTypes = new();

        [ObservableProperty]
        private string _searchingClassesTypesDisplay = string.Empty;

        [ObservableProperty]
        private decimal? _costFrom;

        [ObservableProperty]
        private decimal? _costTo;

        [ObservableProperty]
        private int? _classesAmountFrom;

        [ObservableProperty]
        private int? _classesAmountTo;


        public SubscriptionCardViewModel(OperationType operationType, SubscriptionDTO? subscriptionDTO = null, int? maxId = null)
        {
            if (subscriptionDTO != null)
            {
                _id = subscriptionDTO.Id;
                _name = subscriptionDTO.Name;
                _classesType = subscriptionDTO.ClassesType.Type;
                _cost = subscriptionDTO.Cost;
                _classesAmount = subscriptionDTO.ClassesAmount;
            }
            if (SearchDTO != null)
            {
                _id = SearchDTO.Id;
                _name = SearchDTO.Name ?? string.Empty;
                _classesType = SearchDTO.ClassesType ?? string.Empty;
                _cost = SearchDTO.Cost;
                _classesAmount = SearchDTO.ClassesAmount;
                _idFrom = SearchDTO.IdFrom;
                _idTo = SearchDTO.IdTo;
                _costFrom = SearchDTO.CostFrom;
                _costTo = SearchDTO.CostTo;
                _classesAmountFrom = SearchDTO.ClassesAmountFrom;
                _classesAmountTo = SearchDTO.ClassesAmountTo;
                if (SearchDTO.ClassesTypes != null)
                    SearchingClassesTypes = new ObservableCollection<ClassesTypeDTO>(SearchDTO.ClassesTypes);
            }
            _ids = maxId.HasValue ? Enumerable.Range(1, maxId.Value).ToList() : new List<int>();
            OperationType = operationType;
        }

        public SubscriptionDTO GetSubscriptionDTO()
        {
            return new SubscriptionDTO(
                Id ?? 0,
                Name,
                ClassesTypesDTO.FirstOrDefault(ct => ct.Type == ClassesType) ?? throw new ArgumentNullException(nameof(ClassesType)),
                Cost ?? throw new ArgumentNullException(nameof(Cost)),
                ClassesAmount ?? throw new ArgumentNullException(nameof(ClassesAmount)));
        }

        public SubscriptionSearchDTO GetSearchDTO()
        {
            SearchDTO = new SubscriptionSearchDTO(
                Id: Id,
                Name: string.IsNullOrWhiteSpace(Name) ? null : Name,
                ClassesType: string.IsNullOrWhiteSpace(ClassesType) ? null : ClassesType,
                Cost: Cost,
                ClassesAmount: ClassesAmount,
                IdFrom: IdFrom,
                IdTo: IdTo,
                CostFrom: CostFrom,
                CostTo: CostTo,
                ClassesAmountFrom: ClassesAmountFrom,
                ClassesAmountTo: ClassesAmountTo,
                ClassesTypeIds: SearchingClassesTypes?.Select(ct => ct.Id).ToList(),
                ClassesTypes: SearchingClassesTypes?.ToList()
            );
            return SearchDTO;
        }
    }
}
