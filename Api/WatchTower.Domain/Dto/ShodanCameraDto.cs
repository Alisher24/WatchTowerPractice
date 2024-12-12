namespace WatchTower.Domain.Dto;

using Entity;

public record ShodanCameraDto(
    Guid Id,
    string Ip,
    string CountryName,
    string CountryCode,
    string City,
    double Latitude,
    double Longitude)
{
    public static ShodanCameraDto ToShodanCameraDto(ShodanCamera shodanCamera) =>
        new ShodanCameraDto(
            shodanCamera.Id,
            shodanCamera.Ip,
            shodanCamera.CountryName,
            shodanCamera.CountryCode,
            shodanCamera.City,
            shodanCamera.Latitude,
            shodanCamera.Longitude);
};