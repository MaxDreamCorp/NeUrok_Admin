using System.Media;
using System.Windows.Media.Imaging;
using Notification.Core;
using Notification.Wpf;

namespace NeUrokAdmin.WPF.Services
{
    public class NotificationService : Application.Interfaces.INotificationService
    {
        public void ShowToastNotification(string title, string message)
        {
            var im = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/LogoIcon.png"));
            var content = new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Notification,
                TrimType = NotificationTextTrimType.AttachIfMoreRows,
                RowsCount = 20,
                LeftButtonContent = "Ок",
                Icon = im
            };

            var notificatioManager = new NotificationManager();
            SystemSounds.Beep.Play();
            notificatioManager.Show(content, expirationTime: TimeSpan.FromSeconds(10));
        }
    }
}
