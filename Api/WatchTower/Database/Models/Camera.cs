namespace WatchTower.Database.Models;

public class Camera
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Ip { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }

    public User? User { get; set; }
}