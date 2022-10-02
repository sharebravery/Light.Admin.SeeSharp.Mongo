using AutoMapper;
using Light.Admin.CSharp.Dtos;
using Light.Admin.CSharp.Models;

namespace Light.Admin.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }

    }
}
