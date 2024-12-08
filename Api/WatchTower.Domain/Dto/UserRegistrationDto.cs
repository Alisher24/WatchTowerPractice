namespace WatchTower.Domain.Dto;

public class UserRegistrationDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}