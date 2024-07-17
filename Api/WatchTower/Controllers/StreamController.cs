using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Authorize]
[Route("stream")]
public class StreamController (StreamService streamService)
    : ControllerBase
{
    [HttpPost("start-stream")]
    public Task<IActionResult> StartStream([FromBody] CameraDto cameraDto)
    {
        var streamUrl = $"rtsp://{cameraDto.UserName}:{cameraDto.Password}@{cameraDto.Ip}";

        var result = streamService.FfmpegStarter(streamUrl, cameraDto.Ip!);

        return Task.FromResult<IActionResult>(result.IsSuccess ? Ok(cameraDto.UserName) : BadRequest(result.ErrorMessage));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> WebSocketConnection(string id)
    {
        var context = HttpContext;
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine(id);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ip = await streamService.GetIpCameraAsync(int.Parse(id), userId);
            if (ip.IsNullOrEmpty())
            {
                return NoContent();
            }
            await streamService.StreamVideo(webSocket, context.RequestAborted, ip);
        }

        return Ok();
    }
}