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
                    UserName = s.UserName,
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

    public async Task<BaseResult<CameraDto>> GetCameraByNameAsync(string name, int userId)
    {
        try
        {
            var camera = await dbContext.Cameras
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Name == name);
            
            if (camera == null)
            {
                return new BaseResult<CameraDto>
                {
                    ErrorMessage = $"Camera with ip: {name} not found"
                };
            }

            var cameraDto = new CameraDto()
            {
                Id = camera.Id,
                Ip = camera.Ip,
                Name = camera.Name,
                UserName = camera.UserName,
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
                UserName = cameraDto.UserName,
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

    public async Task<BaseResult> UpdateCameraAsync(CameraDto cameraDto)
    {
        try
        {
            var camera = await dbContext.Cameras
                .FirstOrDefaultAsync(x => x.Id == cameraDto.Id);

            if (camera != null)
            {
                camera.Name = cameraDto.Name;
                camera.UserName = cameraDto.UserName;
                camera.Ip = cameraDto.Ip;
                camera.Password = cameraDto.Password;
                
                dbContext.Update(camera);
                await dbContext.SaveChangesAsync();
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
            var camera = await dbContext.Cameras
                .FirstOrDefaultAsync(x => x.Id == id);

            if (camera != null)
            {
                dbContext.Remove(camera);
                await dbContext.SaveChangesAsync();
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