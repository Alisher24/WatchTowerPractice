using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using WatchTower.Common;
using WatchTower.Common.Result;
using WatchTower.Database;

namespace WatchTower.Services;

public class StreamService(
    IConfiguration configuration,
    ILogger<StreamService> logger,
    WatchTowerDbContext dbContext)
{
    
    private static readonly ConcurrentDictionary
        <string, Ffmpeg> FfmpegProcesses = new();
    private static readonly ConcurrentDictionary
        <string, List<(WebSocket, CancellationToken)>> WebSockets = new();

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
            if (!FfmpegProcesses.ContainsKey(ip))
            {
                Ffmpeg ffmpeg = new Ffmpeg()
                {
                    FfmpegProcess = StartFfmpeg(url)
                };
                var status = FfmpegProcesses.TryAdd(ip, ffmpeg);
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

    private async Task StreamingVideoForAll(string ip)
    {
        try
        {
            var output = FfmpegProcesses[ip]
                .FfmpegProcess.StandardOutput.BaseStream;
            logger.LogInformation("Ffmpeg process status = {status}",
                output.CanRead);

            while ((FfmpegProcesses[ip].BytesRead = await output.ReadAsync(
                       FfmpegProcesses[ip].Buffer, 0,
                       FfmpegProcesses[ip].Buffer.Length)) > 0)
            {

                foreach (var valueTuple in WebSockets[ip])
                {
                    await valueTuple.Item1.SendAsync(new ArraySegment<byte>(
                            FfmpegProcesses[ip].Buffer, 0,
                            FfmpegProcesses[ip].BytesRead),
                        WebSocketMessageType.Binary,
                        true, valueTuple.Item2);
                }
            }

        }
        catch (Exception e)
        {
            logger.LogError(e, "");
        }
        finally
        {
            FfmpegProcesses[ip].Status = false;
        }
    }

    public async Task StreamVideo(
        WebSocket webSocket, CancellationToken cancellationToken, string ip)
    {
        try
        {
            FfmpegProcesses[ip].Listeners++;
            var startStatus = true;
            if (WebSockets.ContainsKey(ip))
            {
                logger.LogInformation("listeners: {listeners}", FfmpegProcesses[ip].Listeners);
                WebSockets[ip].Add((webSocket, cancellationToken));
                while (!cancellationToken.IsCancellationRequested && FfmpegProcesses[ip].Status)
                {
                    
                }

                startStatus = false;

            }
            else
            {
                WebSockets.TryAdd(ip, [(webSocket, cancellationToken)]);
            }
            if (FfmpegProcesses[ip].Status == false)
            {
                if (startStatus)
                {
                    FfmpegProcesses[ip].FfmpegProcess.Start();
                }
                FfmpegProcesses[ip].Status = true;
                await StreamingVideoForAll(ip);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "");
        }
        finally
        {
            if (WebSockets.ContainsKey(ip))
            {
                WebSockets[ip].Remove((webSocket, cancellationToken));
                if (WebSockets[ip].Count == 0)
                {
                    WebSockets.Remove(ip, out _);
                    logger.LogInformation("websocket closed");
                }
            }
            if (FfmpegProcesses.ContainsKey(ip))
            {
                FfmpegProcesses[ip].Listeners--;
                logger.LogInformation("listeners: {listeners}", FfmpegProcesses[ip].Listeners);
                if (FfmpegProcesses[ip].Listeners == 0)
                {
                    FfmpegProcesses[ip].FfmpegProcess.Kill();
                    FfmpegProcesses.Remove(ip, out _);
                }
            }
        }
    }

    public async Task<string> GetIpCameraAsync(int id, int userId)
    {
        var ipCamera = await dbContext.Cameras
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync(c => c.Id == id);
        return ipCamera!.Ip;
    }
}