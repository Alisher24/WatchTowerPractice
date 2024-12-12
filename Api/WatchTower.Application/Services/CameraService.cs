using WatchTower.Domain.Dto;
using WatchTower.Domain.Entity;
using WatchTower.Domain.Shared;
using WatchTower.Infrastructure;

namespace WatchTower.Application.Services;

public class CameraService(BaseRepository repository)
{
    public async Task<Result<List<CameraDto>>> GetAllAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var cameras = userResult.Value.Cameras.Select(CameraDto.ToCameraDto).ToList();

        return cameras;
    }

    public async Task<Result<CameraDto>> GetByIdAsync(
        Guid userId,
        Guid cameraId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var camera = userResult.Value.Cameras.FirstOrDefault(c => c.Id == cameraId);
        if (camera is null)
            return Errors.General.NotFound($"Camera with id: {cameraId}");

        return CameraDto.ToCameraDto(camera);
    }

    public async Task<Result<CameraDto>> GetByTitleAsync(
        string title,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var camera = userResult.Value.Cameras.FirstOrDefault(c => c.Title == title);
        if (camera is null)
            return Errors.General.NotFound($"Camera with title {title}");

        return CameraDto.ToCameraDto(camera);
    }

    public async Task<Result<CameraDto>> RegisterAsync(
        CameraRegistrationDto cameraDto,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var cameraId = Guid.NewGuid();
        var camera = new Camera(
            userResult.Value, 
            cameraId, 
            cameraDto.Title, 
            cameraDto.Ip, 
            cameraDto.Name,
            cameraDto.Password);
        await repository.AddCameraAsync(camera, cancellationToken);

        return CameraDto.ToCameraDto(camera);
    }

    public async Task<Result> UpdateAsync(
        Guid userId,
        CameraDto cameraDto,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var result = await repository.UpdateCameraAsync(userResult.Value, cameraDto, cancellationToken);

        return result;
    }

    public async Task<Result> DeleteAsync(
        Guid userId,
        Guid cameraId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var result = await repository.DeleteCameraAsync(userResult.Value, cameraId, cancellationToken);

        return result;
    }
}