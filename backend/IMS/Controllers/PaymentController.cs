using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/<PaymentController>/:paymentId

        /// <summary>
        /// Get payment
        /// </summary>
        /// <remarks>
        /// Gets the payment
        /// </remarks>
        /// <returns>A Payment</returns>
        /// <response code="200">Returns the payment</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpGet("{paymentId}")]
        [Produces("application/json")]
        [ProducesResponseType<PaymentDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPayment(Guid paymentId)
        {
            var payment = await _paymentService.GetAsync(paymentId);
            return Ok(payment);
        }

        // GET: api/<PaymentController>

        /// <summary>
        /// Get payments
        /// </summary>
        /// <remarks>
        /// Gets the payments
        /// </remarks>
        /// <returns>List of Payments</returns>
        /// <response code="200">Returns the payments</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<PaymentsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetPayments([FromQuery] PaymentsQuery query)
        {
            var payments = await _paymentService.GetAllAsync(query);
            return Ok(payments);
        }
    }
}
