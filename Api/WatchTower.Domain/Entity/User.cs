using WatchTower.Domain.Shared;

namespace WatchTower.Domain.Entity;

public class User : Entity<Guid>
{
    //ef core
    private User(Guid id) : base(id)
    {
    }

    public User(Guid id,
        string name,
        string email,
        string password) : base(id)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? Key { get; set; }

    public bool IsActive { get; set; }

    public string? Token { get; set; }

    public string Password { get; set; } = default!;

    public List<Camera> Cameras { get; set; } = [];
}