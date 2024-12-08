using Microsoft.AspNetCore.Mvc;
using WatchTower.Application.Services;
using WatchTower.Infrastructure;

namespace WatchTower.Api.Controllers;

[ApiController]
public class StreamsController(
    StreamService streamService,
    CameraService cameraService,
    BaseRepository repository) : ApplicationController
{
    [HttpGet("{id:guid}")]
    public async Task<IResult> WebSocketConnection(
        [FromRoute] Guid id,
        [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByTokenAsync(token, cancellationToken);
        if (user is null)
            return Results.Unauthorized();

        var ffmpeg = await streamService.StartFfmpeg(user.Id, id, cancellationToken);
        var context = HttpContext;
        try
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                /*var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);*/
                var camera = await cameraService.GetByIdAsync(user.Id, id, cancellationToken);
                if (camera.IsFailure)
                    return Results.NoContent();

                await streamService.StreamVideo(webSocket, camera.Value.Ip, context.RequestAborted);

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