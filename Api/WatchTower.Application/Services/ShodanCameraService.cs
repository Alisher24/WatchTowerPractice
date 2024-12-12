namespace WatchTower.Application.Services;

using Domain.Dto;
using Domain.Shared;
using Infrastructure;

public class ShodanCameraService(BaseRepository repository, ShodanProvider shodanProvider)
{
    public async Task<Result<ShodanCameraDto>> GetShodanCameraByIdAsync(
        Guid userId,
        Guid shodanCameraId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var result = await repository.GetShodanCameraAsync(
            userResult.Value,
            shodanCameraId,
            cancellationToken);
        if (result.IsFailure)
            return result.ErrorList;

        return ShodanCameraDto.ToShodanCameraDto(result.Value);
    }
    
    public async Task<Result<List<ShodanCameraDto>>> GetShodanCamerasAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var result = await repository.GetShodanCamerasAsync(
            userResult.Value,
            cancellationToken);

        return result.Value.Select(ShodanCameraDto.ToShodanCameraDto).ToList();
    }
    
    public async Task<Result<List<ShodanCameraDto>>> GetShodanCamerasWithPaginationAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var result = await repository
            .GetShodanCamerasWithPaginationAsync(userResult.Value, page, pageSize, cancellationToken);

        return result.Value.Select(ShodanCameraDto.ToShodanCameraDto).ToList();
    }

    public async Task<Result> RegisterShodanCamerasAsync(
        Guid userId,
        string? city,
        string? country,
        uint? offset,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        var shodanCamerasResult = await shodanProvider.GetCamerasAsync(
            userResult.Value,
            city,
            country,
            offset,
            cancellationToken);
        if (shodanCamerasResult.IsFailure)
            return shodanCamerasResult.ErrorList;

        await repository.AddShodanCamerasAsync(
            userResult.Value,
            shodanCamerasResult.Value, 
            cancellationToken);
        
        return Result.Success();
    }
}