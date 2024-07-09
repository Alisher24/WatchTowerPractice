using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WatchTower.Database;
using WatchTower.Database.Models;

namespace WatchTower.Services;

public class AuthService(WatchTowerDbContext dbContext, IConfiguration configuration)
{
    public async Task<User?> Login(string name, string password)
    {
        var user = await dbContext.Users.Include(user => user.Cameras).FirstOrDefaultAsync(x => x.Name == name);

        if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false) 
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            IssuedAt = DateTime.UtcNow,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        user.Token = tokenHandler.WriteToken(token);
        user.IsActive = true;
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> Register(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task Logout(int userId)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        user.Token = null;
        user.IsActive = false;
        await dbContext.SaveChangesAsync();
    }
}