using WatchTower.Domain.Shared;

namespace WatchTower.Domain.Entity;

public class Camera : Entity<Guid>
{
    //ef core
    private Camera(Guid id) : base(id)
    {
    }

    public Camera(
        User user,
        Guid id,
        string? title,
        string ip,
        string? name,
        string? password) : base(id)
    {
        User = user;
        UserId = user.Id;
        Title = title;
        Ip = ip;
        Name = name;
        Password = password;
    }

    public User User { get; init; } = null!;

    public Guid UserId { get; init; }

    public string? Title { get; set; }

    public string Ip { get; set; } = null!;

    public string? Name { get; set; }

    public string? Password { get; set; }
}