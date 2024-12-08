namespace WatchTower.Domain.Dto;

public record CameraRegistrationDto(string Title, string Ip, string? Name, string? Password)
{
    public required string Title { get; init; } = Title;
    public required string Ip {  get; init; } = Ip;
}