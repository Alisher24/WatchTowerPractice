using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Authorize]
[Route("camera")]
public class CameraController(CameraService cameraService) : ControllerBase
{
    private readonly CameraService _cameraService = cameraService;

    [HttpPost("register-camera")]
    public async Task<IActionResult> RegisterCamera([FromBody] CameraRegistrationDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var camera = await _cameraService.RegisterCameraAsync(dto, userId);
        
        if (camera.IsSuccess)
        {
            return Ok(camera.Data);
        }
        
        return BadRequest(camera.ErrorMessage);
    }

    [HttpGet("get-cameras")]
    public async Task<IActionResult> GetCameras()
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var cameras = await _cameraService.GetCamerasAsync(userId);
        
        if (cameras.IsSuccess)
        {
            if (cameras.Data?.Count == 0)
            {
                return NoContent();
            }
            return Ok(cameras.Data);
        }

        return BadRequest(cameras.ErrorMessage);
    }

    [HttpGet("get-camera-by-title{title}")]
    public async Task<IActionResult> GetCameraByName(string title)
    {
        var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var camera = await _cameraService.GetCameraByNameAsync(title, userId);
        
        if (camera.IsSuccess)
        {
            return Ok(camera.Data);
        }

        return BadRequest(camera.ErrorMessage);
    }

    [HttpPut("update-camera")]
    public async Task<IActionResult> UpdateCameraAsync([FromBody] CameraDto dto)
    {
        var result = await _cameraService.UpdateCameraAsync(dto);
        
        if (result.IsSuccess)
        {
            return Ok();
        }
        Console.WriteLine(result.ErrorMessage);

        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("delete-camera/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _cameraService.DeleteCameraAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.ErrorMessage);
    }
}