using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Notify(string message)
        {
            _logger.LogInformation("Sending notification ");
            _logger.LogDebug("Processing notification request.");
            try
            {
                // Simulate success
                _logger.LogInformation("Notification sent successfully.");
                Console.WriteLine($"Notification: {message}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending notification .");
                return StatusCode(500, "An error occurred.");
            }
        }
    }
}
