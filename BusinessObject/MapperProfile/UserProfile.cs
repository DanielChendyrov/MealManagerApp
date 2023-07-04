using AutoMapper;
using BusinessObject.DTO.Response;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(des => des.UserID, opt => opt.MapFrom(src => src.UserId))
            .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(des => des.DepID, opt => opt.MapFrom(src => src.DepId))
            .ForMember(des => des.CompRoleID, opt => opt.MapFrom(src => src.CompRoleId))
            .ForMember(des => des.SysRoleID, opt => opt.MapFrom(src => src.SysRoleId));
    }
}
