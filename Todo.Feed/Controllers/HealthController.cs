using Microsoft.AspNetCore.Mvc;

namespace Todo.Feed.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}