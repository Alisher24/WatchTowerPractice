using System.Diagnostics;

namespace WatchTower.Common;

public class Ffmpeg
{
    public Process FfmpegProcess { get; init; } = null!;
    public byte[] Buffer { get; set; } = new byte[30720];
    public int BytesRead { get; set; }
    public int Listeners { get; set; } = 0;
    public bool Status { get; set; } = false;
}