using Microsoft.EntityFrameworkCore;
using WatchTower.Database.Models;

namespace WatchTower.Database;

public class WatchTowerDbContext(DbContextOptions<WatchTowerDbContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Camera> Cameras { get; set; }
}