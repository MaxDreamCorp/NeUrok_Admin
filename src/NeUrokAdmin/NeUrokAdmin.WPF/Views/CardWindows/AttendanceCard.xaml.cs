using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Application.Features.AttendanceOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для AttendanceCard.xaml
    /// </summary>
    public partial class AttendanceCard : Window
    {
        public AttendanceCardViewModel ViewModel { get; set; } = null!;

        public event EventHandler<AttendanceDTO>? AttendanceChanged;

        private readonly IMediator _mediator;
        private readonly IDialogService _dialogService;

        public AttendanceCard(IMediator mediator, IDialogService dialogService)
        {
            InitializeComponent();
            _mediator = mediator;
            _dialogService = dialogService;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            var statuses = await _mediator.Send(new GetAttendanceStatusesQuery());
            ViewModel.AttendanceStatusesDTO = statuses;
        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckFields()) return;

            var dto = ViewModel.GetAttendanceDTO();

            if (dto.AttendanceStatus == null || dto.Price == null || dto.TeacherShare == null) return;

            var cmd = new UpdateAttendanceCommand(
                dto.Id,
                dto.IsComplited,
                dto.AttendanceStatus.Id,
                dto.Price.Value,
                dto.TeacherShare.Value);

            try
            {
                await _mediator.Send(cmd);
                AttendanceChanged?.Invoke(this, dto);
                Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool CheckFields()
        {
            if (!ViewModel.IsCompleted)
            {
                _dialogService.ShowWarning("Занятие не отмечено как проведенное");
                return false;
            }
            if (ViewModel.Status == null)
            {
                _dialogService.ShowWarning("Статус не выбран");
                return false;
            }
            if (ViewModel.Price == null)
            {
                _dialogService.ShowWarning("Цена занятия не выбрана");
                return false;
            }
            if (ViewModel.TeacherShare == null)
            {
                _dialogService.ShowWarning("Доля преподавателю не выбрана");
                return false;
            }
            return true;
        }
    }
}
