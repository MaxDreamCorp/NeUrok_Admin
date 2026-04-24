namespace NeUrokAdmin.WPF.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string msg, string title);
        void ShowError(string error, string title = "Ошибка");
        void ShowWarning(string warning, string title = "Внимание");
        bool AskQuetion(string question, string title = "Подтверждение");
    }
}
