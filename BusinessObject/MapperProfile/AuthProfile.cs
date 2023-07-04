using AutoMapper;
using BusinessObject.DTO.Request;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class AuthProfile : Profile
{
    public AuthProfile()
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
    }
}
