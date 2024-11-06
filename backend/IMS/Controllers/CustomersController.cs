using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IMS.Extensions;
using ServiceLayer.Provider;
using ServiceLayer.Service;
using ServiceLayer.Extensions;

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Customer")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _config;

        public CustomersController(ICustomerService customerService, IConfiguration config)
        {
            _customerService = customerService;
            _config = config;
        }

        // POST api/<CustomersController>

        /// <summary>
        /// New customer
        /// </summary>
        /// <remarks>
        /// Create a new customer
        /// 
        /// Sample request:
        ///
        ///     POST api/customer
        ///     {
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "phoneNumber": "+233200000000",
        ///         "email": "johndoe@email.com",
        ///         "identificationNumber": "GHA-0000000000-0"
        ///     }
        ///
        /// </remarks>
        /// <returns>A Customer</returns>
        /// <response code="201">Returns the customer</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<CustomerDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] NewCustomerDto newCustomer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var customer = await _customerService.CreateAsync(newCustomer, username);
            return Created(string.Empty, customer);
        }

        // GET: api/<CustomersController>/:customerId

        /// <summary>
        /// Get customer
        /// </summary>
        /// <remarks>
        /// Gets the customer
        /// </remarks>
        /// <returns>A Customer</returns>
        /// <response code="200">Returns the customer</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet("{customerId}")]
        [Produces("application/json")]
        [ProducesResponseType<CustomerDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customer = await _customerService.GetAsync(customerId);
            return Ok(customer);
        }

        // GET: api/<CustomersController>

        /// <summary>
        /// Get customers
        /// </summary>
        /// <remarks>
        /// Gets the customers
        /// </remarks>
        /// <returns>List of Customers</returns>
        /// <response code="200">Returns the customers</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<CustomersDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomersQuery query)
        {
            var customers = await _customerService.GetAllAsync(query);
            return Ok(customers);
        }

        // PUT api/<ArticleController>/:customerId
        /// <summary>
        /// Update a customer
        /// </summary>
        /// <remarks>
        /// Updates a customer
        /// 
        /// Sample request:
        ///
        ///     PUT api/customer/:customerId
        ///     {
        ///         "firstName": "Joe",
        ///     }
        ///     
        /// </remarks>
        /// <returns>A Customer</returns>
        /// <response code="200">Returns a customer</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Error</response>
        [Authorize]
        [HttpPut("{customerId}")]
        [Produces("application/json")]
        [ProducesResponseType<CustomerDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle(Guid customerId, [FromBody] UpdatedCustomerDto updatedCustomerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var article = await _customerService.UpdateAsync(customerId, updatedCustomerDto, username);
            return Ok(article);
        }
    }
}
