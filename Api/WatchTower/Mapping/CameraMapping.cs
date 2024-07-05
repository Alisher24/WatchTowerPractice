using AutoMapper;
using ReactApp1.Server.Models;
using WatchTower.Server.DTO;

namespace WatchTower.Server.Mapping
{
    public class CameraMapping : Profile
    {
        public CameraMapping() 
        {
            CreateMap<Camera, CameraDto>().ReverseMap();
        }
    }
}
