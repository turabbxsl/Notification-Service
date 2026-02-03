using Notification.Api.Queues;
using Notification.Api.Models;

namespace Notification.API.Services.Background
{
    public class NotificationWorker : BackgroundService
    {
        private readonly NotificationQueue _queue;
        private readonly NotificationProcessor _processor;

        public NotificationWorker(NotificationQueue queue)
        {
            _queue = queue;
            _processor = new NotificationProcessor(queue);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            for (int i = 1; i <= 1; i++)
            {
                var name = $"Worker-{i}";
                new Thread(() => _processor.ProcessNotifications(name)).Start();
            }

            return Task.CompletedTask;
        }
    }

}
