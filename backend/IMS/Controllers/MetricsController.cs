using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Metrics")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricService _metricService;

        public MetricsController(IMetricService metricService)
        {
            _metricService = metricService;
        }

        // GET: api/<BillsController>

        /// <summary>
        /// Get dashboard metrics
        /// </summary>
        /// <remarks>
        /// Gets the dashboard metrics
        /// </remarks>
        /// <returns>List of Metrics</returns>
        /// <response code="200">Returns the dashboard metrics</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet("dashboard")]
        [Produces("application/json")]
        [ProducesResponseType<List<MetricDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var response = await _metricService.GetDashboardMetricsAsync();
            return Ok(response);
        }
    }
}
