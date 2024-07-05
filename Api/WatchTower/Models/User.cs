using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchTower.Server.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        public User(string name, string? email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = false;
        public string? Token { get; set; }
        public string Password { get; set; }


        public List<Camera> Cameras { get; set; } = new();
    }
}
