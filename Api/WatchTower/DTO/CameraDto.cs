using WatchTower.Database.Models;

namespace WatchTower.DTO;

public class CameraDto
{
    public string? Ip {  get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
        
    public static CameraDto FromModel (Camera camera)
    {
        return new CameraDto()
        {
            Ip = camera.Ip,
            Name = camera.Name,
            Password = camera.Password
        };
    }
}