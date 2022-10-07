using AutoMapper;
using Light.Admin.Dtos;
using Light.Admin.Mongo;

namespace Light.Admin.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCreateDto, User>().ReverseMap();
            //CreateMap<UserDto, User>().ReverseMap();
        }

    }
}
