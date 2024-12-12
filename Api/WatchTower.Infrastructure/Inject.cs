using Microsoft.Extensions.DependencyInjection;
using WatchTower.Infrastructure.Ffmpeg;

namespace WatchTower.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<FfmpegProvider>();
        services.AddScoped<BaseRepository>();
        services.AddScoped<ShodanProvider>();

        return services;
    }
}