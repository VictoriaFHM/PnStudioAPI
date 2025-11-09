using Microsoft.AspNetCore.Mvc;

namespace PnStudioAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    // GET /healthz
    [HttpGet("/healthz")]
    public IActionResult Get() => Ok(new
    {
        ok = true,
        name = "PnStudioAPI",
        utc = DateTime.UtcNow,
        version = typeof(HealthController).Assembly.GetName().Version?.ToString()
    });
}