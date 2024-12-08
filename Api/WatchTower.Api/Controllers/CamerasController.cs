using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchTower.Application.Services;
using WatchTower.Domain.Dto;

namespace WatchTower.Api.Controllers;

[Authorize]
public class CamerasController(CameraService cameraService) : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Register(
        [FromBody] CameraRegistrationDto dto,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await cameraService.RegisterAsync(dto, userId, cancellationToken);

        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await cameraService.GetAllAsync(userId, cancellationToken);
        
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpGet("{title}")]
    public async Task<ActionResult> GetByTitle(
        [FromRoute] string title,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await cameraService.GetByTitleAsync(title, userId, cancellationToken);

        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromBody] CameraDto dto,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await cameraService.UpdateAsync(userId, dto, cancellationToken);

        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await cameraService.DeleteAsync(userId, id, cancellationToken);

        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }
}