using Microsoft.EntityFrameworkCore;
using WatchTower.Common.Result;
using WatchTower.Database;
using WatchTower.Database.Models;
using WatchTower.DTO;

namespace WatchTower.Services;

public class CameraService(WatchTowerDbContext dbContext)
{
    public async Task<BaseResult<List<CameraDto>>> GetCamerasAsync(int userId)
    {
        try
        {
            var cameras = await dbContext.Cameras
                .Where(x => x.UserId == userId)
                .Select(s => new CameraDto()
                {
                    Id = s.Id,
                    Ip = s.Ip,
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

    public async Task<BaseResult<CameraDto>> GetCameraByIpAsync(string ip, int userId)
    {
        try
        {
            var camera = await dbContext.Cameras
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Ip == ip);

            var cameraDto = new CameraDto()
            {
                Id = camera.Id,
                Ip = camera.Ip,
                Name = camera.Name,
                Password = camera.Password
            };

            if (cameraDto == null)
            {
                return new BaseResult<CameraDto>
                {
                    ErrorMessage = $"Camera with ip: {ip} not found"
                };
            }

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
                Ip = cameraDto.Ip,
                Name = cameraDto.Name,
                Password = cameraDto.Password,
                UserId = userId
            };

            await dbContext.AddAsync(camera);
            await dbContext.SaveChangesAsync();

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
}