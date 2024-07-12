﻿using System.Diagnostics;
using System.Net.WebSockets;
using WatchTower.Common.Result;

namespace WatchTower.Services;

public class StreamService(ILogger<StreamService> logger, IConfiguration configuration)
{
    private static Process? _ffmpegProcess;
    private static HashSet<string> _webSockets = new HashSet<string>();

    public BaseResult<string> StartSream(string streamUrl)
    {
        logger.LogInformation("Received request to start stream with URL: {StreamUrl}", streamUrl);

        if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
        {
            logger.LogInformation("Stopping existing FFmpeg process.");
            _ffmpegProcess.Kill();
        }

        try
        {
            var ffmpegPath = configuration["FFMPEG:Path"];
            var arguments = $"-i {streamUrl} -f mpegts -codec:v mpeg1video pipe:1";
            logger.LogInformation("Starting FFmpeg process with arguments: {Arguments}", arguments);

            _ffmpegProcess = new Process
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

            _ffmpegProcess.Start();
            logger.LogInformation("FFmpeg process started.");

            return new BaseResult<string>()
            {
                Data = GenerationWebSocket()
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

    private string GenerationWebSocket()
    {
        string uniquePath = Guid.NewGuid().ToString();
        while (_webSockets.Contains(uniquePath))
        {
            uniquePath = Guid.NewGuid().ToString();
        }

        _webSockets.Add(uniquePath);
        Console.WriteLine(uniquePath);

        return uniquePath;
    }

    public BaseResult<string> StopStream()
    {
        if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
        {
            _ffmpegProcess.Kill();
            logger.LogInformation("FFmpeg process stopped.");
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
    
    public static async Task StreamVideo(WebSocket webSocket)
    {
        var buffer = new byte[4096];

        try
        {
            await using var output = GetStream();
            Console.WriteLine("teper tut");

            int bytesRead;
            while ((bytesRead = await output.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                if (webSocket.State != WebSocketState.Open)
                    break;
                
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            Console.WriteLine("CloseWebsocket");
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stream ended", CancellationToken.None);
        }
        finally
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stream ended", CancellationToken.None);
        }
    }
    
    private static Stream GetStream()
    {
        return _ffmpegProcess?.StandardOutput.BaseStream!;
    }
}