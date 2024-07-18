using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(AdminService adminService): ControllerBase
{
    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await adminService.GetUsersAsync();

        if (users.IsSuccess)
        {
            if (users.Data == null)
            {
                return NoContent();
            }

            return Ok(users);
        }

        return BadRequest(users.ErrorMessage);
    }

    [HttpGet("get-user-cameras")]
    public async Task<IActionResult> GetUserCameras(int userId)
    {
        var cameras = await adminService.GetUserCamerasAsync(userId);
        
        if (cameras.IsSuccess)
        {
            if (cameras.Data == null)
            {
                return NoContent();
            }

            return Ok(cameras);
        }

        return BadRequest(cameras.ErrorMessage);
    }
}