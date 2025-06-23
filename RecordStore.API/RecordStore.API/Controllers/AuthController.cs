using Microsoft.AspNetCore.Mvc;
using RecordStore.Core.Dtos;
using RecordStore.Infrastructure.Services;
//using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using RecordStore.Database.Entities;

namespace RecordStore.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Logs in a user and returns a JWT token",
            Description = "Authenticates a user with email and password, returning a JWT token for authorization."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid email or password")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Registers a new user",
            Description = "Creates a new user with the provided details."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Registration successful", typeof(User))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Email already exists")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}