namespace WatchTower.DTO;

public class CameraRegistrationDto
{
    public required string Title { get; set; }
    public required string Ip {  get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
}