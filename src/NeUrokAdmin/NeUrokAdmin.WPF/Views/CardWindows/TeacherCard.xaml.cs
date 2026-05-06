using System.Windows;
using MediatR;
using NeUrokAdmin.Application.Features.TeacherOperations.Commands;
using NeUrokAdmin.WPF.Interfaces;
using NeUrokAdmin.WPF.Views.ViewModels.Cards;

namespace NeUrokAdmin.WPF.Views.CardWindows
{
    /// <summary>
    /// Логика взаимодействия для TeacherCard.xaml
    /// </summary>
    public partial class TeacherCard : Window
    {
        private readonly IDialogService _dialogService;
        private readonly IMediator _mediator;

        public TeacherCardViewModel ViewModel { get; set; } = null!;

        public TeacherCard(IDialogService dialogService, IMediator mediator)
        {
            InitializeComponent();
            _dialogService = dialogService;
            _mediator = mediator;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Id == null)
                return;

            var result = _dialogService.AskQuetion("Вы уверены, что хотите удалить данного педагога?");
            if (!result)
                return;

            var cmd = new RemoveTeacherCommand(ViewModel.Id.Value);

            try
            {
                await _mediator.Send(cmd);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
            }
        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            switch (ViewModel.OperationType)
            {
                case Enums.OperationType.Create:
                    result = await CreateCourse();
                    break;
                case Enums.OperationType.Edit:
                    result = await UpdateCourse();
                    break;
            }

            if (result)
            {
                DialogResult = true;
                Close();
            }
        }

        private async Task<bool> CreateCourse()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите создать нового педагога?"))
                return false;

            if (!CheckFields())
                return false;

            try
            {
                var cmd = new CreateTeacherCommand(
                    ViewModel.Fullname,
                    ViewModel.IndividualLessonsShare ?? throw new InvalidOperationException("Доля индивидуальных занятий не может быть пустой"),
                    ViewModel.Notes);
                await _mediator.Send(cmd);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private async Task<bool> UpdateCourse()
        {
            if (!_dialogService.AskQuetion("Вы уверены, что хотите изменить педагога?"))
                return false;

            if (!CheckFields())
                return false;

            try
            {
                var cmd = new UpdateTeacherCommand(
                    ViewModel.Id ?? throw new InvalidOperationException("Id не может быть пустым"),
                    ViewModel.Fullname,
                    ViewModel.IndividualLessonsShare ?? throw new InvalidOperationException("Доля индивидуальных занятий не может быть пустой"),
                    ViewModel.Notes);
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
            if (string.IsNullOrWhiteSpace(ViewModel.Fullname))
            {
                _dialogService.ShowWarning("ФИО не может быть пустым");
                return false;
            }


            return true;
        }
    }
}
