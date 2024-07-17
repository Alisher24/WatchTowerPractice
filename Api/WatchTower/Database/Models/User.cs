using Microsoft.EntityFrameworkCore;

namespace WatchTower.Database.Models;

[Index(nameof(Email), IsUnique = true)]
public class User(string name, string email, string password)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public bool IsActive { get; set; } = false;
    public string? Token { get; set; }
    public string Password { get; set; } = password;


    public List<Camera> Cameras { get; set; } = new();
}