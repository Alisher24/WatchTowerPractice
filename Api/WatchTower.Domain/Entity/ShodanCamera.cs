namespace WatchTower.Domain.Entity;

using Shared;

public class ShodanCamera : Entity<Guid>
{
    //ef core
    private ShodanCamera(Guid id) : base(id)
    {
    }

    public ShodanCamera(
        User user,
        Guid id,
        string ip,
        string countryName,
        string countryCode,
        string city,
        double latitude,
        double longitude) : base(id)
    {
        User = user;
        UserId = user.Id;
        Ip = ip;
        CountryName = countryName;
        CountryCode = countryCode;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }

    public User User { get; init; } = null!;

    public Guid UserId { get; init; } = default!;

    public string Ip { get; init; } = null!;

    public string CountryName { get; init; } = null!;

    public string CountryCode { get; init; } = null!;

    public string City { get; init; } = null!;

    public double Latitude { get; init; } = default!;

    public double Longitude { get; init; } = default!;
}