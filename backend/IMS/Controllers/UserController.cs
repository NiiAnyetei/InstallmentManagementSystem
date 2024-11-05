using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service;
using ServiceLayer.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "User and Authentication")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IConfiguration _config;

        public UserController(IUserService userServices, IConfiguration config)
        {
            _userServices = userServices;
            _config = config;
        }
        
        // POST api/<UserController>/login

        /// <summary>
        /// Existing user login
        /// </summary>
        /// <remarks>
        /// Login for existing user
        /// 
        /// Sample request:
        ///
        ///     POST api/user/login
        ///     {
        ///         "email": "jake@jake.jake",
        ///         "password": "jakejake"
        ///     }
        ///
        /// </remarks>
        /// <returns>Tokens</returns>
        /// <response code="200">Returns tokens</response>
        /// <response code="400">Bad request</response>
        /// <response code="422">Error</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType<LoginUserResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tokens = await _userServices.LoginAsync(loginUser);
            return Ok(tokens);
        }

        // POST api/<UserController>

        /// <summary>
        /// New user signup
        /// </summary>
        /// <remarks>
        /// Register a new user
        /// 
        /// Sample request:
        ///
        ///     POST api/user
        ///     {
        ///         "username": "Jacob",
        ///         "email": "jake@jake.jake",
        ///         "password": "jakejake"
        ///     }
        ///
        /// </remarks>
        /// <returns>A User</returns>
        /// <response code="201">Returns the user</response>
        /// <response code="400">Bad request</response>
        /// <response code="422">Error</response>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateUser([FromBody] NewUserDto newUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userServices.CreateAsync(newUser);
            return Created(string.Empty, user);
        }

        // POST api/<UserController>/refresh

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <remarks>
        /// Refresh token for logged in user
        /// 
        /// Sample request:
        ///
        ///     POST api/user/refresh
        ///     {
        ///         "accessToken": "string",
        ///         "refreshToken": "string"
        ///     }
        ///
        /// </remarks>
        /// <returns>Refresh Token</returns>
        /// <response code="200">Returns the Refresh Token</response>
        /// <response code="400">Bad request</response>
        /// <response code="422">Error</response>
        [AllowAnonymous]
        [HttpPost("refresh")]
        [Produces("application/json")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshToken([FromBody] NewRefreshTokenDto newRefreshTokenDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var refreshToken = await _userServices.RefreshTokenAsync(newRefreshTokenDto);
            return Ok(refreshToken);
        }

        // GET: api/<UserController>

        /// <summary>
        /// Get current user
        /// </summary>
        /// <remarks>
        /// Gets the currently logged-in user
        /// </remarks>
        /// <returns>A User</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpGet("me")]
        [Produces("application/json")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUser()
        {
            var username = User.GetUsername();
            var user = await _userServices.GetAsync(username);
            return Ok(user);
        }

        // PUT: api/<UserController>

        /// <summary>
        /// Update current user
        /// </summary>
        /// <remarks>
        /// Updates details of the currently logged-in user
        /// 
        /// Sample request:
        ///
        ///     PUT api/user
        ///     {
        ///         "email": "jake@jake.jake",
        ///         "bio": "I like to skateboard",
        ///         "image": "https://i.stack.imgur.com/xHWG8.jpg"
        ///     }
        /// </remarks>
        /// <returns>A User</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Error</response>
        [Authorize]
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Error>(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdatedUserDto updatedUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var user = await _userServices.UpdateAsync(username, updatedUser);
            return Ok(user);
        }
    }
}
