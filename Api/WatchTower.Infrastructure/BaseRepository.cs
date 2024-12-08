using Microsoft.EntityFrameworkCore;
using WatchTower.Domain.Dto;
using WatchTower.Domain.Entity;
using WatchTower.Domain.Shared;

namespace WatchTower.Infrastructure;

public class BaseRepository(ApplicationDbContext dbContext)
{
    public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users.Include(u => u.Cameras)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (result is null)
            return Errors.General.NotFound("User");

        return result;
    }
    
    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (result is null)
            return Errors.General.NotFound("User");

        return result;
    }

    public async Task<User?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Token == token, cancellationToken);

        return result;
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTokenAsync(
        User user, 
        string token, 
        CancellationToken cancellationToken)
    {
        user.Token = token;
        user.IsActive = true;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddCameraAsync(Camera camera, CancellationToken cancellationToken)
    {
        await dbContext.Cameras.AddAsync(camera, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> UpdateCameraAsync(User user, CameraDto cameraDto, CancellationToken cancellationToken)
    {
        var camera = user.Cameras.FirstOrDefault(c => c.Id == cameraDto.Id);
        if (camera is null)
            return Errors.General.NotFound("Camera");

        camera.Title = cameraDto.Title;
        camera.Ip = cameraDto.Ip;
        camera.Name = cameraDto.Name;
        camera.Password = cameraDto.Password;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> DeleteCameraAsync(User user, Guid cameraId, CancellationToken cancellationToken)
    {
        var camera = user.Cameras.FirstOrDefault(c => c.Id == cameraId);
        if (camera is null)
            return Errors.General.NotFound("Camera");

        dbContext.Cameras.Remove(camera);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}