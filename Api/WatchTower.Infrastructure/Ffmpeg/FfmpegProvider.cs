using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WatchTower.Infrastructure.Ffmpeg;

public class FfmpegProvider(
    ILogger<FfmpegProvider> logger,
    IConfiguration configuration)
{
    private const string FfmpegPath = "FFMPEG:Path";

    private readonly ConcurrentDictionary<string, Ffmpeg> _ffmpegProcesses = new();
    private readonly ConcurrentDictionary<string, List<(WebSocket, CancellationToken)>> _webSockets = new();

    public void AddProcess(string ip, string url)
    {
        if (!_ffmpegProcesses.ContainsKey(ip))
        {
            var ffmpeg = new Ffmpeg
            {
                FfmpegProcess = StartFfmpeg(url)
            };
            var status = _ffmpegProcesses.TryAdd(ip, ffmpeg);
            logger.LogInformation("Add status ffmpeg process {status}", status);
        }
    }

    public async Task Stream(
        WebSocket webSocket,
        string ip,
        CancellationToken cancellationToken)
    {
        try
        {
            _ffmpegProcesses[ip].Listeners++;
            var startStatus = true;
            if (_webSockets.ContainsKey(ip))
            {
                logger.LogInformation("listeners: {listeners}", _ffmpegProcesses[ip].Listeners);
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
                    _ffmpegProcesses[ip].FfmpegProcess.Start();

                _ffmpegProcesses[ip].Status = true;
                await StreamVideoForAll(ip);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "");
        }
        finally
        {
            if (_webSockets.ContainsKey(ip))
            {
                _webSockets[ip].Remove((webSocket, cancellationToken));
                if (_webSockets[ip].Count == 0)
                {
                    _webSockets.Remove(ip, out _);
                    logger.LogInformation("websocket closed");
                }
            }

            if (_ffmpegProcesses.ContainsKey(ip))
            {
                _ffmpegProcesses[ip].Listeners--;
                logger.LogInformation("listeners: {listeners}", _ffmpegProcesses[ip].Listeners);
                if (_ffmpegProcesses[ip].Listeners == 0)
                {
                    _ffmpegProcesses[ip].FfmpegProcess.Kill();
                    _ffmpegProcesses.Remove(ip, out _);
                }
            }
        }
    }

    private Process StartFfmpeg(string streamUrl)
    {
        logger.LogInformation("Received request to start stream with URL: {StreamUrl}", streamUrl);

        try
        {
            var ffmpegPath = configuration[FfmpegPath];

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
            logger.LogError(ex.Message);
            throw;
        }
    }

    private async Task StreamVideoForAll(string ip)
    {
        try
        {
            var output = _ffmpegProcesses[ip]
                .FfmpegProcess.StandardOutput.BaseStream;
            logger.LogInformation("Ffmpeg process status = {status}",
                output.CanRead);

            while ((_ffmpegProcesses[ip].BytesRead =
                       await output.ReadAsync(_ffmpegProcesses[ip].Buffer
                           .AsMemory(0, _ffmpegProcesses[ip].Buffer.Length))) > 0)
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
            logger.LogError(e, "");
        }
        finally
        {
            _ffmpegProcesses[ip].Status = false;
        }
    }
}