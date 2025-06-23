using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace RecordStore.API.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet("test-connection")]
        [SwaggerOperation(
            Summary = "Tests API connection",
            Description = "Returns a simple message to confirm the API is reachable."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Connection successful", typeof(string))]
        public async Task<IActionResult> Get()
        {
            return Ok("Testing connection...");
        }

        [HttpGet("protected")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Access protected endpoint",
            Description = "Requires a valid JWT token in the Authorization header (Bearer). Returns the authenticated user's email."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Access granted", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
        public IActionResult GetProtected()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok($"Protected endpoint accessed by {email}");
        }
    }
}