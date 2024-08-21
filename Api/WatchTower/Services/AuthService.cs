using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WatchTower.Common.Result;
using WatchTower.Database;
using WatchTower.Database.Models;
using WatchTower.DTO;

namespace WatchTower.Services;

public class AuthService(WatchTowerDbContext dbContext, IConfiguration configuration)
{
    private readonly WatchTowerDbContext _dbContext = dbContext;
    private readonly IConfiguration _configuration = configuration;

    public async Task<BaseResult<UserDto>?> Login(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false) 
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]!);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            IssuedAt = DateTime.UtcNow,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        user.Token = tokenHandler.WriteToken(token);
        user.IsActive = true;
        await _dbContext.SaveChangesAsync();
        
        return new BaseResult<UserDto>()
        {
            Data = new UserDto(user.Name, user.Token)
        };
    }

    public async Task<User> Register(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserWithToken(string token)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Token == token);

        return user;
    }
}