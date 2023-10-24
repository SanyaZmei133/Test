using AutoMapper;
using TestVegastar.Models;
using TestVegastar.ModelsDTO;

namespace TestVegastar.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Usergroup, UsergroupDto>();
            CreateMap<UsergroupDto, Usergroup>();
            CreateMap<Userstate, UserstateDto>();
            CreateMap<UserstateDto, Userstate>();
        }
    }
}
