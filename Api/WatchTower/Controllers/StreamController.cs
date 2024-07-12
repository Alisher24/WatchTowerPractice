using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var streamUrl = $"rtsp://{cameraDto.Name}:{cameraDto.Password}@{cameraDto.Ip}";

        var result = streamService.StartSream(streamUrl);

        return Task.FromResult<IActionResult>(result.IsSuccess ? Ok(cameraDto.Ip+result.Data) : BadRequest(result.ErrorMessage));
    }

    [HttpGet("stop-stream")]
    public IActionResult StopStream()
    {
        var result = streamService.StopStream();
        
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }
}