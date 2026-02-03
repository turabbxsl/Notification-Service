using Microsoft.AspNetCore.Mvc;
using Notification.Api.Queues;
using Notification.API.Dtos;

namespace Notification.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationQueue _queue;

        public NotificationController(NotificationQueue queue)
        {
            _queue = queue;
        }


        [HttpPost]
        public IActionResult Send([FromBody] CreateNotificationDto model)
        {
            _queue.Enqueue(new Notification.Api.Models.Notification()
            {
                Id = Random.Shared.Next(1000, 9999),
                UserId = model.UserId,
                Message = model.Message,
                Type = model.Type,
                Priority = model.Priority
            });

            return Ok("Added to Queue");
        }
    }
}
