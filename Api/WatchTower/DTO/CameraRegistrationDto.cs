namespace WatchTower.DTO;

public class CameraRegistrationDto
{
    public required string Name { get; set; }
    public required string Ip {  get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}