using WatchTower.Database.Models;

namespace WatchTower.DTO;

public class CameraDto
{
    public required int Id { get; set; }
    public required string Name { get; init; }
    public required string Ip {  get; init; }
    public string? UserName { get; init; }
    public string? Password { get; init; }
        
    public static CameraDto FromModel (Camera camera)
    {
        return new CameraDto()
        {
            Id = camera.Id,
            Ip = camera.Ip,
            Name = camera.Name,
            UserName = camera.Name,
            Password = camera.Password
        };
    }
}