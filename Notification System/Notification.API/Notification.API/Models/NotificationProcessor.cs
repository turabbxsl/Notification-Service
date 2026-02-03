using Notification.Api.Channels;
using Notification.Api.Enums;
using Notification.Api.Queues;

namespace Notification.Api.Models
{
    /// <summary>
    /// NotificationProcessor – Consumer logic
    /// Purpose: Receives and sends notifications from the Queue, implements retry logic
    /// </summary>
    public class NotificationProcessor
    {
        private readonly NotificationQueue _queue;
        private readonly Dictionary<NotificationType, INotificationChannel> _channels;
        private const int MaxRetry = 3;

        public NotificationProcessor(NotificationQueue queue)
        {
            _queue = queue;
            _channels = new() {
                { NotificationType.Email,new EmailChannel() } ,
                { NotificationType.Sms,new SMSChannel() } ,
                { NotificationType.Push,new PushChannel() }
            };
        }

        public void ProcessNotifications(string threadName)
        {
            while (true)
            {
                var notification = _queue.Dequeue(); // get notification from queue
                if (notification == null) break; // shutdown

                SendNotificationWithRetry(notification, threadName);
            }
        }

        private void SendNotificationWithRetry(Notification notification, string threadName)
        {
            var channel = _channels[notification.Type];
            while (notification.RetryCount < MaxRetry)
            {
                bool success = channel.Send(notification);
                if (success)
                {
                    notification.Status = Status.Sent;
                    return;
                }

                notification.RetryCount++;
            }

            notification.Status = Status.Failed; // Maximum retry has been passed.
        }

    }
}
