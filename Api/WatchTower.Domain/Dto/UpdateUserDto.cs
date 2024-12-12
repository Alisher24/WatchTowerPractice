namespace WatchTower.Domain.Dto;

public record UpdateUserDto(
    string Name,
    string Email,
    string? Key);