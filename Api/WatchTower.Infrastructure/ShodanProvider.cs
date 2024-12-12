namespace WatchTower.Infrastructure;

using Domain.Entity;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Shodan.Client;
using Shodan.Search;

public class ShodanProvider(ILogger<ShodanProvider> logger)
{
    private const int Port = 554;

    public async Task<Result<List<ShodanCamera>>> GetCamerasAsync(
        User user,
        string? city,
        string? country,
        uint? offset,
        CancellationToken cancellationToken)
    {
        try
        {
            var shodan = new ClientFactory(user.Key);
            var shodanClient = shodan.GetFullClient();
            var searchQuery = new ShodanSearchQuery
            {
                Port = Port,
                Country = country ?? "RU",
                City = city ?? "Moscow",
                Parameters = new ShodanSearchParameters
                {
                    Offset = offset ?? 0
                }
            };

            var searchResult = await shodanClient.Search(searchQuery, cancellationToken);
            var result = searchResult.Services
                .Select(s => new ShodanCamera(
                    user,
                    Guid.NewGuid(),
                    s.IPStr,
                    s.Location.CountryName,
                    s.Location.CountryCode,
                    s.Location.City,
                    s.Location.Latitude!.Value,
                    s.Location.Longitude!.Value)).ToList();

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to get cameras from shodan");
            return Error.Failure("shodan.get.cameras", "Fail to get cameras from shodan");
        }
    }
}