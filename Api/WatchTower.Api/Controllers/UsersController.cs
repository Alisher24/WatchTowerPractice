namespace WatchTower.Api.Controllers;

using System.Security.Claims;
using Application.Services;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class UsersController(UserService userService) : ApplicationController
{
    [HttpPut]
    public async Task<ActionResult> Update(
        [FromBody] UpdateUserDto updateUserDto,
        CancellationToken cancellationToken = default)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await userService.UpdateAsync(
            userId,
            updateUserDto,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }
}