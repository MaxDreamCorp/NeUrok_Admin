using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.ClientOperations.Commands;
using NeUrokAdmin.Application.Features.ClientOperations.Queries;
using NeUrokAdmin.Application.Features.CourseOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Services;
using NeUrokAdmin.WPF.Views.Selectors;
using NeUrokAdmin.WPF.Views.ViewModels;
using NeUrokAdmin.WPF.Views.ViewModels.Selectors;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для ClientCard.xaml
    /// </summary>
    public partial class ClientCard : Window
    {
        private readonly IMediator _mediator;
        private readonly NavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public ClientCardViewModel ViewModel { get; set; } = null!;

        public ClientCard(IMediator mediator, NavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var qry = new GetAllClientsStatusesQuery();
            var statuses = await _mediator.Send(qry);

            ViewModel.ClientStatusesDTO = statuses;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            switch (ViewModel.OperationType)
            {
                case Enums.OperationType.Create:
                    result = await CreateClient();
                    break;
                case Enums.OperationType.Read:
                    break;
                case Enums.OperationType.Edit:
                    result = await UpdateClient();
                    break;
                case Enums.OperationType.Filter:
                    break;
                default:
                    break;
            }

            if (result)
            {
                DialogResult = true;
                Close();
            }
        }

        private async void SelectCoursesBtn_Click(object sender, RoutedEventArgs e)
        {
            var allCourses = await _mediator.Send(new GetAllCoursesQuery());

            var vm = new CoursesSelectorViewModel(allCourses, ViewModel.WishedCourses.ToList());
            var selectorWindow = _navigationService.GetWindow<CoursesSelectorWindow>();
            selectorWindow.ViewModel = vm;
            selectorWindow.CoursesSelected += SelectorWindow_CoursesSelected;
            selectorWindow.ShowDialog();
        }

        private void SelectorWindow_CoursesSelected(object? sender, List<Domain.DTOs.CourseDTO> e)
        {
            ViewModel.WishedCourses = new(e);
            ViewModel.WishedCoursesDisplay = string.Join(", ", ViewModel.WishedCourses.Select(c => c.Name));
        }

        private async Task<bool> CreateClient()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать нового клиента?"))
                return false;

            if (!CheckFields())
                return false;

            ClientDTO? dto;
            try
            {
                dto = ViewModel.GetClientDTO();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }

            var cmd = new CreateClientCommand(
            dto.ChildFullname,
            dto.BirthDate,
            dto.RegistrationDate,
            dto.Grade,
            dto.Status.Id,
            dto.ParentName,
            dto.Phone,
            dto.WishedCourses?.Select(c => c.Id).ToList(),
            dto.Notes,
            dto.AdditionalPhones);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }
        
        private async Task<bool> UpdateClient()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите сохранить изменения?"))
                return false;

            if (!CheckFields())
                return false;

            ClientDTO? dto;
            try
            {
                dto = ViewModel.GetClientDTO();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }

            var cmd = new UpdateClientCommand(
                dto.Id,
            dto.ChildFullname,
            dto.BirthDate,
            dto.RegistrationDate,
            dto.Grade,
            dto.Status.Id,
            dto.ParentName,
            dto.Phone,
            dto.WishedCourses?.Select(c => c.Id).ToList(),
            dto.Notes,
            dto.AdditionalPhones);

            try
            {
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private bool CheckFields()
        {
            if (string.IsNullOrEmpty(ViewModel.ChildFullname))
            {
                _dialogService.ShowWarning("ФИО ребенка не может быть пустым.");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.ParentName))
            {
                _dialogService.ShowWarning("Имя родителя не может быть пустым.");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.Phone))
            {
                _dialogService.ShowWarning("Телефон не может быть пустым.");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.Status))
            {
                _dialogService.ShowWarning("Статус не может быть пустым.");
                return false;
            }
            return true;
        }
    }
}
