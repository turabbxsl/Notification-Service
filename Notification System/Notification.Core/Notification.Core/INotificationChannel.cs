using Notification.Core.Enums;

namespace Notification.Core
{
    public interface INotificationChannel
    {
        NotificationType Type { get; }
        bool Send(Models.Notification notification);
    }

    public class EmailChannel : INotificationChannel
    {
        public NotificationType Type => NotificationType.Email;
        public bool Send(Models.Notification notification)
        {
            // sending email

            Thread.Sleep(500);
            return true;
        }
    }

    public class SMSChannel : INotificationChannel
    {
        public NotificationType Type => NotificationType.Sms;
        public bool Send(Models.Notification notification)
        {
            // sending sms

            Thread.Sleep(300);
            return true;
        }
    }

    public class PushChannel : INotificationChannel
    {
        public NotificationType Type => NotificationType.Push;
        public bool Send(Models.Notification notification)
        {
            Thread.Sleep(200);
            return true;
        }
    }

}
