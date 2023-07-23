namespace WebPerformance.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Text()
    {
        return Ok("Hello world.");
    }
}
