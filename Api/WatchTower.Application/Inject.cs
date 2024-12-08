using Microsoft.Extensions.DependencyInjection;
using WatchTower.Application.Services;

namespace WatchTower.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<CameraService>();
        services.AddScoped<StreamService>();

        return services;
    }
}