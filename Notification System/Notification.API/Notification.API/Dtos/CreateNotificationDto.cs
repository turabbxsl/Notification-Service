using Notification.Api.Enums;

namespace Notification.API.Dtos
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public Priority Priority { get; set; }
    }
}
