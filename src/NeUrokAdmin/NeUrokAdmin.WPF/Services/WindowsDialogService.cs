using System.Windows;
using NeUrokAdmin.WPF.Interfaces;

namespace NeUrokAdmin.WPF.Services
{
    public class WindowsDialogService : IDialogService
    {
        public bool AskQuetion(string question, string title = "Подтверждение") =>
            MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;

        public void ShowError(string error, string title = "Ошибка") =>
            MessageBox.Show(error, title, MessageBoxButton.OK, MessageBoxImage.Error);

        public void ShowMessage(string msg, string title) =>
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowWarning(string warning, string title = "Внимание") =>
            MessageBox.Show(warning, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
