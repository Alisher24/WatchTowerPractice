using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        <string, Ffmpeg> _ffmpegProcesses = new();
    private static readonly ConcurrentDictionary
        <string, List<(WebSocket, CancellationToken)>> _webSockets = new();
    private readonly ILogger<StreamService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;


    private Process StartFfmpeg(string streamUrl)
    {
        _logger.LogInformation("Received request to start stream with URL: {StreamUrl}", streamUrl);

        try
        {
            var ffmpegPath = _configuration["FFMPEG:Path"];

            var arguments = $"-i {streamUrl} -f mpegts -codec:v mpeg1video pipe:1";
            _logger.LogInformation("Starting FFmpeg process with arguments: {Arguments}", arguments);

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

            _logger.LogInformation("FFmpeg process started.");
            return process;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            throw;
        }
    }

    public async Task<BaseResult<string>> FfmpegStarter(int id)
    {
        try
        {
            var camera = await dbContext.Cameras
                .FirstOrDefaultAsync(x => x.Id == id);

            var ip = camera!.Ip;
            var url = camera.Name.IsNullOrEmpty() ? $"rtsp://{camera.Ip}" 
                : $"rtsp://{camera.Name}:{camera.Password}@{camera.Ip}";
            
            if (!_ffmpegProcesses.ContainsKey(ip))
            {
                Ffmpeg ffmpeg = new Ffmpeg()
                {
                    FfmpegProcess = StartFfmpeg(url)
                };
                var status = _ffmpegProcesses.TryAdd(ip, ffmpeg);
                _logger.LogInformation("Add status ffmpeg process {status}", status);
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
            var output = _ffmpegProcesses[ip]
                .FfmpegProcess.StandardOutput.BaseStream;
            _logger.LogInformation("Ffmpeg process status = {status}",
                output.CanRead);

            while ((_ffmpegProcesses[ip].BytesRead = await output.ReadAsync(
                       _ffmpegProcesses[ip].Buffer, 0,
                       _ffmpegProcesses[ip].Buffer.Length)) > 0)
            {

                foreach (var valueTuple in _webSockets[ip])
                {
                    await valueTuple.Item1.SendAsync(new ArraySegment<byte>(
                            _ffmpegProcesses[ip].Buffer, 0,
                            _ffmpegProcesses[ip].BytesRead),
                        WebSocketMessageType.Binary,
                        true, valueTuple.Item2);
                }
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        finally
        {
            _ffmpegProcesses[ip].Status = false;
        }
    }

    public async Task StreamVideo(
        WebSocket webSocket, CancellationToken cancellationToken, string ip)
    {
        try
        {
            _ffmpegProcesses[ip].Listeners++;
            var startStatus = true;
            if (_webSockets.ContainsKey(ip))
            {
                _logger.LogInformation("listeners: {listeners}", _ffmpegProcesses[ip].Listeners);
                _webSockets[ip].Add((webSocket, cancellationToken));
                while (!cancellationToken.IsCancellationRequested && _ffmpegProcesses[ip].Status)
                {
                    
                }

                startStatus = false;

            }
            else
            {
                _webSockets.TryAdd(ip, [(webSocket, cancellationToken)]);
            }
            if (_ffmpegProcesses[ip].Status == false)
            {
                if (startStatus)
                {
                    _ffmpegProcesses[ip].FfmpegProcess.Start();
                }
                _ffmpegProcesses[ip].Status = true;
                await StreamingVideoForAll(ip);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
        }
        finally
        {
            if (_webSockets.ContainsKey(ip))
            {
                _webSockets[ip].Remove((webSocket, cancellationToken));
                if (_webSockets[ip].Count == 0)
                {
                    _webSockets.Remove(ip, out _);
                    _logger.LogInformation("websocket closed");
                }
            }
            if (_ffmpegProcesses.ContainsKey(ip))
            {
                _ffmpegProcesses[ip].Listeners--;
                _logger.LogInformation("listeners: {listeners}", _ffmpegProcesses[ip].Listeners);
                if (_ffmpegProcesses[ip].Listeners == 0)
                {
                    _ffmpegProcesses[ip].FfmpegProcess.Kill();
                    _ffmpegProcesses.Remove(ip, out _);
                }
            }
        }
    }

    public async Task<string?> GetIpCameraAsync(int id, int userId)
    {
        var ipCamera = await dbContext.Cameras
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync(c => c.Id == id);
        return ipCamera?.Ip;
    }
}