using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using WatchTower.Server.Models;

namespace WatchTower.Server.BusinessLogic
{
    public class WatchTowerDbContext : DbContext
    {
        public WatchTowerDbContext(DbContextOptions<WatchTowerDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Camera> Cameras { get; set; }
    }
}
