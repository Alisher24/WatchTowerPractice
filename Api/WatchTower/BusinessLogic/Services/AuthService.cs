namespace WatchTower.Server.BusinessLogic.Implementation
{
    using BCrypt.Net;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Security.Claims;
    using WatchTower.Server.Models;
    using WatchTower.Server.BusinessLogic.Interface;
    using Microsoft.EntityFrameworkCore;

    public class AuthService : IAuthService
    {
        private readonly WatchTowerDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public AuthService(WatchTowerDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User?> Login(string name, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);

            if (user == null || BCrypt.Verify(password, user.Password) == false) 
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            user.Token = tokenHandler.WriteToken(token);
            user.IsActive = true;

            return user;
        }

        public async Task<User> Register(User user)
        {
            user.Password = BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
