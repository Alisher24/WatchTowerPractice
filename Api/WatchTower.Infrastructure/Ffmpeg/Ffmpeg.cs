using System.Diagnostics;

namespace WatchTower.Infrastructure.Ffmpeg;

public class Ffmpeg
{
    public Process FfmpegProcess { get; init; } = default!;
    public byte[] Buffer { get; } = new byte[30720];
    public int BytesRead { get; set; }
    public int Listeners { get; set; }
    public bool Status { get; set; }
}