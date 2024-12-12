namespace WatchTower.Api.Requests;

public record RegisterShodanCamerasRequest(
    string? City,
    string? Country,
    uint? Offset);