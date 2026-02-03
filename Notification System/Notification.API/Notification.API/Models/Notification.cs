using Notification.Api.Enums;

namespace Notification.Api.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public int RetryCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
