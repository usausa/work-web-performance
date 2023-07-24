namespace WebPerformance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using Smart.Data.Accessor;

using WebPerformance.Accessors;

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
    public async ValueTask<IActionResult> Delay([FromRoute] int timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);

        return Ok("Hello world.");
    }

    [HttpGet("{timeout}")]
    public IActionResult Sleep([FromRoute] int timeout)
    {
        Thread.Sleep(timeout);

        return Ok("Hello world.");
    }

    [HttpGet("{timeout}")]
    [EnableRateLimiting("fixed")]
    public async ValueTask<IActionResult> Limit([FromRoute] int timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);

        return Ok("Hello world.");
    }

    [HttpGet("{id}")]
    public async ValueTask<IActionResult> Query([FromRoute] string id, [FromServices] IAccessorResolver<IDataAccessor> accessor)
    {
        var entity = await accessor.Accessor.QueryAsync(id).ConfigureAwait(false);
        if (entity is null)
        {
            return NotFound();
        }

        return Ok("Hello world.");
    }
}
