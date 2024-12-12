namespace WatchTower.Api.Controllers;

using System.Security.Claims;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Requests;

[Authorize]
public class ShodanCamerasController(ShodanCameraService shodanCameraService) : ApplicationController
{
    [HttpGet("all")]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await shodanCameraService.GetShodanCamerasAsync(
            userId,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await shodanCameraService.GetShodanCameraByIdAsync(
            userId,
            id,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetWithPagination(
        [FromQuery] GetShodanCamerasWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await shodanCameraService.GetShodanCamerasWithPaginationAsync(
            userId,
            request.Page,
            request.PageSize,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpGet("register")]
    public async Task<ActionResult> Register(
        [FromQuery] RegisterShodanCamerasRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await shodanCameraService.RegisterShodanCamerasAsync(
            userId,
            request.City,
            request.Country,
            request.Offset,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }
}