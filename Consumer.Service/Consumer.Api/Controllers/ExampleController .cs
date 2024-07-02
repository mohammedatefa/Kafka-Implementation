using Consumer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController(NotificationService notificationService, ILogger<ExampleController> logger) : ControllerBase
    {
        private readonly NotificationService _notificationService = notificationService;
        private readonly ILogger<ExampleController> _logger = logger;

        [HttpPost("trigger")]
        public IActionResult TriggerNotification()
        {
            _notificationService.NotifyClients("This is a new notification!");
            _logger.LogInformation("send notification");
            return Ok(new { message = "Notification sent." });
        }
    }
}
