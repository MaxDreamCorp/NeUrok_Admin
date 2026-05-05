using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class TeacherCardViewModel : ObservableObject
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
                        HeaderText = "Создание педагога";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Педагог \"{Fullname}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Педагог \"{Fullname}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = true;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр педагогов";
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
        private string _fullname = string.Empty;

        [ObservableProperty]
        private decimal _individualLessonsShare;

        [ObservableProperty]
        private string? _notes;


        public TeacherCardViewModel(OperationType operationType, TeacherDTO? teacherDTO)
        {
            if (teacherDTO != null)
            {
                _id = teacherDTO.Id;
                _fullname = teacherDTO.Fullname;
                _individualLessonsShare = teacherDTO.IndividualLessonsShare;
                _notes = teacherDTO.Notes;
            }
            OperationType = operationType;
        }
    }
}
