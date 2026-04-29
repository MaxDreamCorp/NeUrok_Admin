using CommunityToolkit.Mvvm.ComponentModel;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Enums;

namespace NeUrokAdmin.WPF.Views.ViewModels.Cards
{
    public partial class CourseCardViewModel : ObservableObject
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
                        HeaderText = "Создание курса";
                        break;
                    case OperationType.Read:
                        IsFilter = false;
                        IsEditable = false;
                        IsDeletable = false;
                        HeaderText = $"Курс \"{Name}\"";
                        break;
                    case OperationType.Edit:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = true;
                        HeaderText = $"Курс \"{Name}\"";
                        break;
                    case OperationType.Filter:
                        IsFilter = false;
                        IsEditable = true;
                        IsDeletable = false;
                        HeaderText = "Фильтр курсов";
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
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        public CourseCardViewModel(OperationType operationType, CourseDTO? course = null)
        {
            if (course != null)
            {
                _id = course.Id;
                _name = course.Name;
            }

            OperationType = operationType;
        }

    }
}
