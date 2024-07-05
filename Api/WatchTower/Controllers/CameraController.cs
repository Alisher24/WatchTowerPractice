using System.Diagnostics;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
/*    [Authorize]*/
[Route("stream")]
public class CameraController(CameraService cameraService, ILogger<CameraController> logger)
    : ControllerBase
{
    private Process? _ffmpegProcess;

    [HttpPost("register-camera")]
    public async Task<IActionResult> RegisterCamera([FromBody] CameraRegistrationDto dto)
    {
        var camera = await cameraService.RegisterCameraAsync(dto);
        if (camera.IsSuccess)
        {
            return Ok(camera.Data);
        }
        return BadRequest(camera.ErrorMessage);
    }

    [HttpGet("start-stream")]
    public IActionResult StartStream([FromQuery] string address, [FromQuery] string username, [FromQuery] string password)
    {
        var streamUrl = $"rtsp://{username}:{password}@{address}";
        logger.LogInformation("Received request to start stream with URL: {StreamUrl}", streamUrl);

        if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
        {
            logger.LogInformation("Stopping existing FFmpeg process.");
            _ffmpegProcess.Kill();
        }

        var ffmpegPath = "C:\\Users\\katie\\AppData\\Local\\Microsoft\\WinGet\\Packages\\Gyan.FFmpeg_Microsoft.Winget.Source_8wekyb3d8bbwe\\ffmpeg-7.0.1-full_build\\bin\\ffmpeg.exe";
        var arguments = $"-i {streamUrl} -f mpegts -codec:v mpeg1video pipe:1";
        logger.LogInformation("Starting FFmpeg process with arguments: {Arguments}", arguments);

        _ffmpegProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        _ffmpegProcess.Start();
        logger.LogInformation("FFmpeg process started.");

        return Ok("Stream started");
    }

    [HttpGet("stop-stream")]
    public IActionResult StopStream()
    {
        if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
        {
            _ffmpegProcess.Kill();
            logger.LogInformation("FFmpeg process stopped.");
            return Ok("Stream stopped");
        }

        return BadRequest("No stream to stop.");
    }

    private Stream GetStream()
    {
        return _ffmpegProcess?.StandardOutput.BaseStream!;
    }
    
    private async Task StreamVideo(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[4096];
        int bytesRead;

        try
        {
            using var output = GetStream();

            if (output == null)
            {
                context.Response.StatusCode = 500;
                return;
            }

            while ((bytesRead = await output.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                if (webSocket.State != WebSocketState.Open)
                    break;

                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
        finally
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stream ended", CancellationToken.None);
        }
    }
}