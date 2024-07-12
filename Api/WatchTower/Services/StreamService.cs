using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Xml.Linq;
using WatchTower.Common.Result;

namespace WatchTower.Services;

public class StreamService(
    IConfiguration configuration,
    ILogger<StreamService> logger)
{
    private static ConcurrentDictionary<string, Process> _ffmpegProcesses = new();

    private Process StartFfmpeg(string streamUrl)
    {
        logger.LogInformation("Received request to start stream with URL: {StreamUrl}", streamUrl);

        try
        {
            var ffmpegPath = configuration["FFMPEG:Path"];

            var arguments = $"-i {streamUrl} -f mpegts -codec:v mpeg1video pipe:1";
            logger.LogInformation("Starting FFmpeg process with arguments: {Arguments}", arguments);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            logger.LogInformation("FFmpeg process started.");
            return process;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError(ex, "");
            throw;
        }
    }

    public BaseResult<string> FfmpegStarter(string url, string ip)
    {
        try
        {
            if (!_ffmpegProcesses.ContainsKey(ip))
            {
                var ffmpegProces = StartFfmpeg(url);
                var status = _ffmpegProcesses.TryAdd(ip, ffmpegProces);
                logger.LogInformation("Add status ffmpeg process {status}", status);
            }

            return new BaseResult<string>()
            {
                Data = url
            };
        }
        catch (Exception ex)
        {
            return new BaseResult<string>()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public BaseResult<string> StopStream(string ip)
    {
        if (_ffmpegProcesses.ContainsKey(ip))
        {
            _ffmpegProcesses[ip].Kill();
            logger.LogInformation("FFmpeg process stopped.");
            _ffmpegProcesses.TryRemove(ip, out _);
            return new BaseResult<string>()
            {
                Data = "Stream stopped"
            };
        }

        return new BaseResult<string>()
        {
            ErrorMessage = "No stream to stop"
        };
    }

    public async Task StreamVideo(WebSocket webSocket, CancellationToken cancellationToken, string ip)
    {
        var buffer = new byte[30720];

        try
        {

            var output = _ffmpegProcesses[ip].StandardOutput.BaseStream;
            logger.LogInformation("Ffmpeg process status = {status}", output.CanRead);

            int bytesRead;

            logger.LogInformation("bytesRead = {b}",
                await output.ReadAsync(buffer, 0, buffer.Length, cancellationToken) > 0);

            while ((bytesRead = await output.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                if (webSocket.State == WebSocketState.Closed || cancellationToken.IsCancellationRequested)
                {
                    logger.LogInformation("webSocket.State = {state} and cancellation token = {token}", webSocket.State,
                        cancellationToken.IsCancellationRequested);
                    break;
                }

                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary,
                    true, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "");
        }
        finally
        {
            logger.LogInformation("Stop stream");
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stream ended", cancellationToken);
        }
    }
}