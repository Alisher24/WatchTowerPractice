using ReactApp1.Server.Models;
using WatchTower.Server.BusinessLogic.Result;
using WatchTower.Server.DTO;

namespace WatchTower.Server.BusinessLogic.Interface
{
    public interface ICameraService
    {
        public Task<List<CameraDto>> GetCamerasAsync(int userId);

        public Task<Camera> GetCameraByIpAsync(string ip);

        public Task<Camera> RegisterCameraAsync(CameraRegistrationDto cameraDto);

    }
}
