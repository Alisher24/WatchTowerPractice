using WatchTower.Common.Result;
using WatchTower.Database;
using WatchTower.Database.Models;
using WatchTower.DTO;

namespace WatchTower.Services;

public class CameraService(WatchTowerDbContext dbContext)
{
    private readonly WatchTowerDbContext _dbContext = dbContext;

    /*public async Task<List<CameraDto>> GetCamerasAsync(int userId)
    {
        List<CameraDto> cameras;
        try
        {
            cameras = await _cameraRepository.GetAll()
                .Where(x => x.UserId == userId)
                .Select(s => new CameraDto()
                {
                    Ip = s.Ip,
                    Name = s.Name,
                    Password = s.Password
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
                
        }

        return new List<Camera>()
        {
                
        };
    }

    public async Task<BaseResult<CameraDto>> GetCameraByIpAsync(string ip)
    {
        CameraDto? camera;
        try
        {
            camera = await _cameraRepository.GetAll()
                .Select(s => new CameraDto()
                {
                    Ip = s.Ip,
                    Name = s.Name,
                    Password = s.Password
                })
                .FirstOrDefaultAsync(x => x.Ip == ip);
        }
        catch (Exception ex)
        {
            return new BaseResult<CameraDto>()
            {
                ErrorMessage = ex.Message
            };
        }

        if (camera == null)
        {
            return new BaseResult<CameraDto>
            {
                ErrorMessage = "Нет камеры"
            };
        }

        return new BaseResult<CameraDto>
        {
            Data = camera
        };
    }*/

    public async Task<BaseResult<CameraDto>> RegisterCameraAsync(CameraRegistrationDto cameraDto)
    {
        try
        {
            var camera = new Camera()
            {
                Ip = cameraDto.Ip,
                Name = cameraDto.Name,
                Password = cameraDto.Password,
                UserId = cameraDto.UserId
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
}