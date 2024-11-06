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
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        // GET: api/<BillsController>/:billId

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
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet("{billId}")]
        [Produces("application/json")]
        [ProducesResponseType<BillDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBill(Guid billId)
        {
            var bill = await _billService.GetAsync(billId);
            return Ok(bill);
        }

        // GET: api/<BillsController>

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
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<BillsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBills([FromQuery] BillsQuery query)
        {
            var bills = await _billService.GetAllAsync(query);
            return Ok(bills);
        }
    }
}
