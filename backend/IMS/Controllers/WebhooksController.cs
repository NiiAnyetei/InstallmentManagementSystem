using IMS.Extensions;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Paystack.DTOs;
using ServiceLayer.Provider;
using ServiceLayer.Service;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Webhooks")]
    public class WebhooksController : ControllerBase
    {
        private readonly ILogger<WebhooksController> _logger;
        private readonly IWebhookProcessor _webhookProcessor;

        public WebhooksController(ILogger<WebhooksController> logger, IWebhookProcessor webhookProcessor)
        {
            _logger = logger;
            _webhookProcessor = webhookProcessor;
        }

        [HttpPost("paystack")]
        public async Task<IActionResult> Paystack(PaystackWebhookDto dto)
        {
            _logger.LogInformation("Paystack Webhook Received: {dto}", dto.ToJson());
            await _webhookProcessor.ProcessWebhookEventAsync(dto);
            return Ok();
        }
    }
}
