namespace WatchTower.Application.Services;

using Domain.Dto;
using Domain.Shared;
using Infrastructure;

public class UserService(BaseRepository repository)
{
    public async Task<Result> UpdateAsync(
        Guid userId,
        UpdateUserDto updateUserDto,
        CancellationToken cancellationToken)
    {
        var userResult = await repository.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return userResult.ErrorList;

        userResult.Value.Name = updateUserDto.Name;
        userResult.Value.Email = updateUserDto.Email;
        userResult.Value.Key = updateUserDto.Key;
        await repository.UpdateAsync(userResult.Value, cancellationToken);

        return Result.Success();
    }
}