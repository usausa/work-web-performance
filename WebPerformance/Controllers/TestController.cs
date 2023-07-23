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

    [HttpGet("{timeout}")]
    public async ValueTask<IActionResult> Delay(int timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);

        return Ok("Hello world.");
    }

    [HttpGet("{timeout}")]
    public IActionResult Sleep(int timeout)
    {
        Thread.Sleep(timeout);

        return Ok("Hello world.");
    }
}
