﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<BaseResult<CameraDto>> GetCameraByIpAsync(string ip)
    {
        try
        {
            var camera = await dbContext.Cameras
                .Select(s => new CameraDto()
                {
                    Ip = s.Ip,
                    Name = s.Name,
                    Password = s.Password
                })
                .FirstOrDefaultAsync(x => x.Ip == ip);

            if (camera == null)
            {
                return new BaseResult<CameraDto>
                {
                    ErrorMessage = $"Камеры с данным ip: {ip} не существует"
                };
            }

            return new BaseResult<CameraDto>()
            {
                Data = camera
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