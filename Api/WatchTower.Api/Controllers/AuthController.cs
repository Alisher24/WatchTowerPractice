using Microsoft.AspNetCore.Mvc;
using WatchTower.Application.Services;
using WatchTower.Domain.Dto;

namespace WatchTower.Api.Controllers;

public class AuthController(AuthService authService) : ApplicationController
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] UserLoginDto user,
        CancellationToken cancellationToken = default)
    {
        var result = await authService.Login(user.Email, user.Password, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(
        [FromBody] UserRegistrationDto userRegistrationDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await authService.Register(userRegistrationDto, cancellationToken);
        if (result.IsFailure)
            return result.ErrorList.ToResponse();

        return Ok(result);
    }
}