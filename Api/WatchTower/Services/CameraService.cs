using Microsoft.EntityFrameworkCore;
using WatchTower.Common.Result;
using WatchTower.Database;
using WatchTower.Database.Models;
using WatchTower.DTO;

namespace WatchTower.Services;

public class CameraService(WatchTowerDbContext dbContext)
{
    private readonly WatchTowerDbContext _dbContext = dbContext;
    public async Task<BaseResult<List<CameraDto>>> GetCamerasAsync(int userId)
    {
        try
        {
            var cameras = await _dbContext.Cameras
                .Where(x => x.UserId == userId)
                .Select(s => new CameraDto()
                {
                    Id = s.Id,
                    Ip = s.Ip,
                    Title = s.Title,
                    Name = s.Name,
                    Password = s.Password
                })
                .ToListAsync();

            return new BaseResult<List<CameraDto>>()
            {
                Data = cameras
            };
        }
        catch (Exception ex)
        {
            return new BaseResult<List<CameraDto>>()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<BaseResult<CameraDto>> GetCameraByNameAsync(string title, int userId)
    {
        try
        {
            var camera = await _dbContext.Cameras
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Title == title);
            
            if (camera == null)
            {
                return new BaseResult<CameraDto>
                {
                    ErrorMessage = $"Camera with title: {title} not found"
                };
            }

            var cameraDto = new CameraDto()
            {
                Id = camera.Id,
                Ip = camera.Ip,
                Title = camera.Title,
                Name = camera.Name,
                Password = camera.Password
            };

            return new BaseResult<CameraDto>()
            {
                Data = cameraDto
            };
        }
        catch (Exception ex)
        {
            return new BaseResult<CameraDto>()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<BaseResult<CameraDto>> RegisterCameraAsync(CameraRegistrationDto cameraDto, int userId)
    {
        try
        {
            var camera = new Camera()
            {
                Title = cameraDto.Title,
                Ip = cameraDto.Ip,
                Name = cameraDto.Name,
                Password = cameraDto.Password,
                UserId = userId
            };

            await _dbContext.AddAsync(camera);
            await _dbContext.SaveChangesAsync();

            return new BaseResult<CameraDto>
            {
                Data = CameraDto.FromModel(camera)
            };
        }
        catch (Exception ex)
        {
            return new BaseResult<CameraDto>()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<BaseResult> UpdateCameraAsync(CameraDto cameraDto)
    {
        try
        {
            var camera = await _dbContext.Cameras
                .FirstOrDefaultAsync(x => x.Id == cameraDto.Id);

            if (camera != null)
            {
                camera.Title = cameraDto.Title;
                camera.Name = cameraDto.Name;
                camera.Ip = cameraDto.Ip;
                camera.Password = cameraDto.Password;
                
                _dbContext.Update(camera);
                await _dbContext.SaveChangesAsync();
            }

            return new BaseResult();
        }
        catch (Exception ex)
        {
            return new BaseResult()
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<BaseResult> DeleteCameraAsync(int id)
    {
        try
        {
            var camera = await _dbContext.Cameras
                .FirstOrDefaultAsync(x => x.Id == id);

            if (camera != null)
            {
                _dbContext.Remove(camera);
                await _dbContext.SaveChangesAsync();
            }

            return new BaseResult();
        }
        catch (Exception ex)
        {
            return new BaseResult()
            {
                ErrorMessage = ex.Message
            };
        }
    }
}