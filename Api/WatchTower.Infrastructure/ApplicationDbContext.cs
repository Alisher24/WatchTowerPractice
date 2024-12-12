using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WatchTower.Domain.Entity;

namespace WatchTower.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    private const string DatabaseConnection = "DefaultConnection";

    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

    public DbSet<User> Users => Set<User>();

    public DbSet<Camera> Cameras => Set<Camera>();

    public DbSet<ShodanCamera> ShodanCameras => Set<ShodanCamera>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DatabaseConnection));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}