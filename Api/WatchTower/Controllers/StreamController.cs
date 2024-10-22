﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WatchTower.DTO;
using WatchTower.Services;

namespace WatchTower.Controllers;

[ApiController]
[Route("stream")]
public class StreamController (StreamService streamService, AuthService authService)
    : ControllerBase
{
    private readonly StreamService _streamService = streamService;
    private readonly AuthService _authService = authService;

    [HttpGet("{id}")]
    public async Task<IResult> WebSocketConnection(string id, [FromQuery] string token)
    {
        var user = await _authService.GetUserWithToken(token);
        if (user == null)
        {
            return Results.Unauthorized();
        }
        var ffmpeg = await _streamService.FfmpegStarter(int.Parse(id));
        var context = HttpContext;
        try
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine(id);
                /*var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);*/
                var ip = await _streamService.GetIpCameraAsync(int.Parse(id), user.Id);
                if (ip.IsNullOrEmpty())
                {
                    return Results.NoContent();
                }
                await _streamService.StreamVideo(webSocket, context.RequestAborted, ip);
                
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest("Not websocket");
            }
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }
}