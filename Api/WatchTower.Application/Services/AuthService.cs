using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WatchTower.Domain.Dto;
using WatchTower.Domain.Entity;
using WatchTower.Domain.Shared;
using WatchTower.Infrastructure;

namespace WatchTower.Application.Services;

public class AuthService(BaseRepository repository, IConfiguration configuration)
{
    private const string KeyPath = "JWT:SecretKey";
    private const string IssuerPath = "JWT:Issuer";
    private const string AudiencePath = "JWT:Audience";

    public async Task<Result<UserDto>> Login(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByEmailAsync(email, cancellationToken);
        if (!userResult.IsSuccess)
            return Errors.General.ValueIsInvalid("User");

        if (BCrypt.Net.BCrypt.Verify(password, userResult.Value.Password) == false)
            return Errors.General.ValueIsInvalid("User");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.Unicode.GetBytes(configuration[KeyPath]!);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userResult.Value.Name),
                new Claim(ClaimTypes.NameIdentifier, userResult.Value.Id.ToString()),
            }),
            IssuedAt = DateTime.UtcNow,
            Issuer = configuration[IssuerPath],
            Audience = configuration[AudiencePath],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescription);
        await repository.UpdateTokenAsync(userResult.Value, tokenHandler.WriteToken(token), cancellationToken);

        return new UserDto(userResult.Value.Name, userResult.Value.Token!);
    }

    public async Task<Result> Register(UserRegistrationDto userRegistrationDto, CancellationToken cancellationToken)
    {
        var userByEmailResult = await repository.GetByEmailAsync(userRegistrationDto.Email, cancellationToken);
        if (userByEmailResult.IsSuccess)
            return Errors.General.ValueIsAlreadyExists("User");

        var password = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);
        var user = new User(Guid.NewGuid(), userRegistrationDto.Name, userRegistrationDto.Email, password);
        await repository.CreateAsync(user, cancellationToken);

        return Result.Success();
    }
}