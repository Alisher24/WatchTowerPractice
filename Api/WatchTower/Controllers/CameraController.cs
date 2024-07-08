using Microsoft.AspNetCore.Mvc;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Route("camera")]
public class CameraController(CameraService cameraService)
    : ControllerBase
{
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

    [HttpGet("get-cameras")]
    public async Task<IActionResult> GetCameras(int userId)
    {
        var cameras = await cameraService.GetCamerasAsync(userId);
        if (cameras.IsSuccess)
        {
            return Ok(cameras.Data);
        }

        return BadRequest(cameras.ErrorMessage);
    }

    [HttpGet("get-camera-by-ip")]
    public async Task<IActionResult> GetCameraByIp(string ip)
    {
        var camera = await cameraService.GetCameraByIpAsync(ip);
        if (camera.IsSuccess)
        {
            return Ok(camera.Data);
        }

        return BadRequest(camera.ErrorMessage);
    }
}