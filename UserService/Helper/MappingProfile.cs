using AutoMapper;

namespace UserService.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.User, DTO.GetUsersDTO>();
            CreateMap<Models.User, DTO.GetDetailUserDTO>();
        }
    }
}
