using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Authorize]
[Route("stream")]
public class StreamController (StreamService streamService)
    : ControllerBase
{
    [HttpGet("start-stream")]
    public async Task<IActionResult> StartStream([FromQuery] string address, [FromQuery] string username, [FromQuery] string password)
    {
        await StreamVideo();
            
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
    
    private async Task StreamVideo()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await streamService.StreamVideo(websocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}