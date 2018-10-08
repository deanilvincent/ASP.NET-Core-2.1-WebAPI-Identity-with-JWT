using AutoMapper;
using MyApp.Dtos;
using MyApp.Models;

namespace MyApp.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<RoleForRegisterDto, Role>();
        }
    }
}
