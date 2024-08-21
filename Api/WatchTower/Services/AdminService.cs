using Microsoft.EntityFrameworkCore;
using WatchTower.Common.Result;
using WatchTower.Database;
using WatchTower.DTO;

namespace WatchTower.Services;

public class AdminService(WatchTowerDbContext dbContext, ILogger<AdminService> _logger)
{
    private readonly WatchTowerDbContext _dbContext = dbContext;
    private readonly ILogger<AdminService> _logger = _logger;

    public async Task<BaseResult<List<UserForAdminDto>>> GetUsersAsync()
    {
        try
        {
            var users = await _dbContext.Users
                .Select(x => new UserForAdminDto(
                    x.Id, x.Name, x.Email, x.Role))
                .ToListAsync();

            return new BaseResult<List<UserForAdminDto>>()
            {
                Data = users
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"");
            return new BaseResult<List<UserForAdminDto>>()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<BaseResult<List<CameraDto>>> GetUserCamerasAsync(int userId)
    {
        try
        {
            var user = await _dbContext.Users
                .Include(user => user.Cameras)
                .FirstOrDefaultAsync(x => x.Id == userId);

            var cameras = user?.Cameras
                .Select(camera => new CameraDto()
                {
                    Id = camera.Id,
                    Ip = camera.Ip,
                    Title = camera.Title,
                    Name = camera.Name,
                    Password = camera.Password
                })
                .ToList();

            return new BaseResult<List<CameraDto>>()
            {
                Data = cameras
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            return new BaseResult<List<CameraDto>>()
            {
                ErrorMessage = ex.Message
            };
        }
    }
}