using Microsoft.AspNetCore.Mvc;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Route("stream")]
public class StreamController (StreamService streamService)
    : ControllerBase
{
    [HttpGet("start-stream")]
    public IActionResult StartStream([FromQuery] string address, [FromQuery] string username, [FromQuery] string password)
    {
        var streamUrl = $"rtsp://{username}:{password}@{address}";

        var result = streamService.StartSream(streamUrl);

        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }

    [HttpGet("stop-stream")]
    public IActionResult StopStream()
    {
        var result = streamService.StopStream();
        
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }
}