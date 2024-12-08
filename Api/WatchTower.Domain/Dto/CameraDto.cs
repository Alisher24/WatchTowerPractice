using WatchTower.Domain.Entity;

namespace WatchTower.Domain.Dto;

public record CameraDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public required string Ip {  get; init; }
    public string? Name { get; init; }
    public string? Password { get; init; }
        
    public static CameraDto ToCameraDto (Camera camera)
    {
        return new CameraDto
        {
            Id = camera.Id,
            Ip = camera.Ip,
            Title = camera.Title,
            Name = camera.Name,
            Password = camera.Password
        };
    }
}