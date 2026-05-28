using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.AttendanceOperations.Commands;
using NeUrokAdmin.Application.Features.AttendanceOperations.Queries;
using NeUrokAdmin.Domain.DTOs;
using NeUrokAdmin.Domain.Enums;
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

        private bool _isCreating;

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

            bool isExcused = (ViewModel.Status == ViewModel.AttendanceStatusesDTO
                                  .Find(s => s.Id == (int)AttendanceStatusEnum.Excused)?.Status);
            CreateWorkOffBtn.Visibility = isExcused ?
                Visibility.Visible :
                Visibility.Collapsed;
        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsWorkingOff && _isCreating)
                await CreateAsync();
            else
                await UpdateAsync();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async Task CreateAsync()
        {
            if (!CheckFieldsOnWOCreating() || ViewModel.NewDatetime == null)
                return;

            var dto = ViewModel.GetAttendanceDTO();

            var newDt = new DateTime(DateOnly.FromDateTime(ViewModel.NewDatetime.Value), 
                new(int.Parse(ViewModel.TimeHours), int.Parse(ViewModel.TimeMinutes)));
            var cmd = new CreateWorkingOffAttendanceCommand(
                ViewModel.Student.Id,
                ViewModel.Datetime,
                newDt,
                dto.Course.Id,
                dto.ClassesType.Id,
                dto.Teacher.Id,
                dto.GroupId);

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

        private async Task UpdateAsync()
        {
            if (!CheckFields()) return;

            var dto = ViewModel.GetAttendanceDTO();

            var cmd = new UpdateAttendanceCommand(
                dto.Id,
                dto.IsComplited,
                dto.AttendanceStatus?.Id ?? 0,
                dto.Price.HasValue ? dto.Price.Value : 0,
                dto.TeacherShare.HasValue ? dto.TeacherShare.Value : 0,
                dto.AbsentCause,
                dto.Notes);

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

        private bool CheckFields()
        {
            if (ViewModel.IsCompleted)
            {

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
            }
            return true;
        }

        private bool CheckFieldsOnWOCreating()
        {
            if (ViewModel.NewDatetime == null)
            {
                _dialogService.ShowWarning("Дата не может быть пустым");
                return false;
            }
            else if (ViewModel.NewDatetime.HasValue && ViewModel.NewDatetime <= ViewModel.Datetime)
            {
                _dialogService.ShowWarning("Дата не может быть раньше исходного занятия");
                return false;
            }
            if (string.IsNullOrEmpty(ViewModel.TimeHours) || string.IsNullOrEmpty(ViewModel.TimeMinutes))
            {
                _dialogService.ShowWarning("Время не может быть пустым");
                return false;
            }
            return true;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bool isExcused = (e.AddedItems.Count > 0 &&
                              e.AddedItems[0]?.ToString() == ViewModel.AttendanceStatusesDTO
                                  .Find(s => s.Id == (int)AttendanceStatusEnum.Excused)?.Status);
            CauseLb.Visibility = isExcused ?
            Visibility.Visible :
            Visibility.Collapsed;
            CauseTxt.Visibility = isExcused ?
                Visibility.Visible :
                Visibility.Collapsed;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Datetime > DateTime.Now)
            {
                _dialogService.ShowWarning("Вы не можете отмечать проведение будущих занятий");
                ViewModel.IsCompleted = false;
                return;
            }
            if (ViewModel.Price.HasValue) return;

            if (ViewModel.IsWorkingOff)
            {
                ViewModel.Price = 0;
                ViewModel.TeacherShare = 0;
                return;
            }
            ViewModel.Price = ViewModel.StudentSubscription.Cost / ViewModel.StudentSubscription.ClassesAmount;

            if (ViewModel.TeacherShare == null)
            {
                if (ViewModel.StudentSubscription.ClassesType.Id == (int)ClassesTypeEnum.Individual)
                    ViewModel.TeacherShare = ViewModel.Teacher.IndividualLessonsShare;
                else
                {
                    if (ViewModel.Teacher.Id == 1)
                        ViewModel.TeacherShare = ViewModel.Price;
                    else
                        ViewModel.TeacherShare = ViewModel.Price * 0.6m;
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Price = null;
            ViewModel.TeacherShare = null;
            ViewModel.Status = null;
            ViewModel.AbsentCause = null;
        }

        private async void CreateWorkOffBtn_Click(object sender, RoutedEventArgs e)
        {
            var workingOffType = await _mediator.Send(new GetAttendanceTypeByIdQuery((int)AttendanceTypeEnum.WorkingOff));
            if (workingOffType == null) return;

            _isCreating = true;
            ViewModel.IsWorkingOff = true;
            ViewModel.Id = 0;

            ViewModel.Type = workingOffType;

            DatetimeTextBox.Visibility = Visibility.Collapsed;
            WorkingOffSetTimePanel.Visibility = Visibility.Visible;

            ViewModel.IsCompleted = false;

            CreateWorkOffBtn.Visibility = Visibility.Collapsed;
        }
    }
}
