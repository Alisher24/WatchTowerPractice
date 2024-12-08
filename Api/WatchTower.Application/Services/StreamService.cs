using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using WatchTower.Domain.Shared;
using WatchTower.Infrastructure;
using WatchTower.Infrastructure.Ffmpeg;

namespace WatchTower.Application.Services;

public class StreamService(
    ILogger<StreamService> logger,
    BaseRepository repository,
    FfmpegProvider provider)
{
    public async Task<Result<string>> StartFfmpeg(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userResult = await repository.GetByIdAsync(userId, cancellationToken);
            if (!userResult.IsSuccess)
                return userResult.ErrorList;

            var camera = userResult.Value.Cameras.FirstOrDefault(c => c.Id == id);
            if (camera is null)
                return Errors.General.NotFound("Camera");

            var ip = camera!.Ip;
            var url = string.IsNullOrEmpty(camera.Name)
                ? $"rtsp://{camera.Ip}"
                : $"rtsp://{camera.Name}:{camera.Password}@{camera.Ip}";

            provider.AddProcess(ip, url);

            return url;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "");

            return Error.Failure("Failed to create connection with ffmpeg", "ffmpeg.start.failure");
        }
    }

    public async Task StreamVideo(
        WebSocket webSocket, string ip, CancellationToken cancellationToken)
    {
        await provider.Stream(webSocket, ip, cancellationToken);
    }
}