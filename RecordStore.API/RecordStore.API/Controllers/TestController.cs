using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RecordStore.API.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet("test-connection")]
        public async Task<IActionResult> Get()
        {
            return Ok("Testing connection...");
        }
    }
}
