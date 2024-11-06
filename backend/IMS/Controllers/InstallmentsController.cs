using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IMS.Extensions;
using ServiceLayer.Service;
using ServiceLayer.External.Paystack.Service;
using ServiceLayer.Extensions;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Installment")]
    public class InstallmentsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IInstallmentService _installmentService;
        private readonly IPaystackService _paystackService;
        private readonly ICustomerService _customerService;
        private readonly IBillService _billService;

        public InstallmentsController(IConfiguration config, IInstallmentService installmentService, IPaystackService paystackService, ICustomerService customerService, IBillService billService)
        {
            _config = config;
            _installmentService = installmentService;
            _paystackService = paystackService;
            _customerService = customerService;
            _billService = billService;
        }

        // POST api/<InstallmentsController>

        /// <summary>
        /// New installment
        /// </summary>
        /// <remarks>
        /// Create a new installment
        /// 
        /// Sample request:
        ///
        ///     POST api/installment
        ///     {
        ///            "customerId": "a6356aae-e248-42a0-87b3-f68a2a92e9be",
        ///            "item": "iPhone X",
        ///            "itemDetails": "64GB | XDFDFSS7",
        ///            "amount": 1000,
        ///            "initialDeposit": 0,
        ///            "cyclePeriod": 1,
        ///            "cycleNumber": 10,
        ///            "paymentChannel": "mtn"
        ///     }
        ///     
        ///     "cyclePeriod": 1, //Daily = 1, Weekly = 7, Monthly = 30,
        ///     "paymentChannel": "mtn" //"mtn" | "atl" | "vod"
        ///
        /// </remarks>
        /// <returns>A Installment</returns>
        /// <response code="201">Returns the installment</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<NewInstallmentDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInstallment([FromBody] NewInstallmentDto newInstallment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var customer = await _customerService.GetAsync(Guid.Parse(newInstallment.CustomerId));
            var installment = await _installmentService.CreateAsync(newInstallment, username);

            return Created(string.Empty, installment);
        }

        // GET: api/<InstallmentsController>/:installmentId

        /// <summary>
        /// Get installment
        /// </summary>
        /// <remarks>
        /// Gets the installment
        /// </remarks>
        /// <returns>A Installment</returns>
        /// <response code="200">Returns the installment</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet("{installmentId}")]
        [Produces("application/json")]
        [ProducesResponseType<InstallmentDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInstallment(Guid installmentId)
        {
            var installment = await _installmentService.GetAsync(installmentId);
            return Ok(installment);
        }

        // GET: api/<InstallmentsController>

        /// <summary>
        /// Get installments
        /// </summary>
        /// <remarks>
        /// Gets the installments
        /// </remarks>
        /// <returns>List of Installments</returns>
        /// <response code="200">Returns the installments</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<InstallmentsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInstallments([FromQuery] InstallmentsQuery query)
        {
            var installments = await _installmentService.GetAllAsync(query);
            return Ok(installments);
        }

        // PUT api/<ArticleController>/:installmentId
        /// <summary>
        /// Update a installment
        /// </summary>
        /// <remarks>
        /// Updates a installment
        /// 
        /// Sample request:
        ///
        ///     PUT api/installment/:installmentId
        ///     {
        ///         "firstName": "Joe",
        ///     }
        ///     
        /// </remarks>
        /// <returns>A Installment</returns>
        /// <response code="200">Returns a installment</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpPut("{installmentId}")]
        [Produces("application/json")]
        [ProducesResponseType<InstallmentDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle(Guid installmentId, [FromBody] UpdatedInstallmentDto updatedInstallmentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var article = await _installmentService.UpdateAsync(installmentId, updatedInstallmentDto, username);
            return Ok(article);
        }
    }
}
