using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Bills")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        // GET: api/<BillController>/:billId

        /// <summary>
        /// Get bill
        /// </summary>
        /// <remarks>
        /// Gets the bill
        /// </remarks>
        /// <returns>A Bill</returns>
        /// <response code="200">Returns the bill</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpGet("{billId}")]
        [Produces("application/json")]
        [ProducesResponseType<BillDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetBill(Guid billId)
        {
            var bill = await _billService.GetAsync(billId);
            return Ok(bill);
        }

        // GET: api/<BillController>

        /// <summary>
        /// Get bills
        /// </summary>
        /// <remarks>
        /// Gets the bills
        /// </remarks>
        /// <returns>List of Bills</returns>
        /// <response code="200">Returns the bills</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<BillsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetBills([FromQuery] BillsQuery query)
        {
            var bills = await _billService.GetAllAsync(query);
            return Ok(bills);
        }
    }
}
