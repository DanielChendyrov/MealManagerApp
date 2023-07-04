using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SignUpDTO, User>()
            .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(des => des.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(des => des.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(des => des.Dep.DepId, opt => opt.MapFrom(src => src.DepID))
            .ForMember(des => des.CompRole.CompRoleId, opt => opt.MapFrom(src => src.CompRoleID))
            .ForMember(des => des.SysRole.SysRoleId, opt => opt.MapFrom(src => src.SysRoleID));

        CreateMap<LogInDTO, User>()
            .ForMember(des => des.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(des => des.Password, opt => opt.MapFrom(src => src.Password));

        CreateMap<User, UserDTO>()
            .ForMember(des => des.UserID, opt => opt.MapFrom(src => src.UserId))
            .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(des => des.DepID, opt => opt.MapFrom(src => src.DepId))
            .ForMember(des => des.CompRoleID, opt => opt.MapFrom(src => src.CompRoleId))
            .ForMember(des => des.SysRoleID, opt => opt.MapFrom(src => src.SysRoleId));
        CreateMap<UserDTO, User>()
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserID))
            .ForMember(des => des.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(des => des.DepId, opt => opt.MapFrom(src => src.DepID))
            .ForMember(des => des.CompRoleId, opt => opt.MapFrom(src => src.CompRoleID))
            .ForMember(des => des.SysRoleId, opt => opt.MapFrom(src => src.SysRoleID));
    }
}
