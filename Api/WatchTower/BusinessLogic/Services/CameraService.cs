using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using WatchTower.Server.BusinessLogic.Interface;
using WatchTower.Server.BusinessLogic.Interfaces;
using WatchTower.Server.BusinessLogic.Result;
using WatchTower.Server.DTO;
using WatchTower.Server.Models;

namespace WatchTower.Server.BusinessLogic.Implementation
{
    public class CameraService
    {
        /*private readonly IBaseRepository<Camera> _cameraRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public CameraService(IBaseRepository<Camera> cameraRepository, IBaseRepository<User> userRepository, IMapper mapper)
        {
            _cameraRepository = cameraRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<CameraDto>> GetCamerasAsync(int userId)
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

                await _cameraRepository.CreateAsync(camera);

                return new BaseResult<CameraDto>
                {
                    Data = _mapper.Map<CameraDto>(camera)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<CameraDto>()
                {
                    ErrorMessage = ex.Message
                };
            }
        }*/
    }
}
