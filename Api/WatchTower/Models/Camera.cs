using System.ComponentModel.DataAnnotations.Schema;
using WatchTower.Server.Models;

namespace ReactApp1.Server.Models
{
    public class Camera
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Ip { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }

        public User? User { get; set; } 
    }
}
