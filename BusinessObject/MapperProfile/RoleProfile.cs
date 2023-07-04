using AutoMapper;
using BusinessObject.DTO;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<CompanyRoleDTO, CompanyRole>()
            .ForMember(des => des.CompRoleId, opt => opt.MapFrom(src => src.CompRoleID))
            .ForMember(des => des.CompRoleName, opt => opt.MapFrom(src => src.CompRoleName));
        CreateMap<CompanyRole, CompanyRoleDTO>()
            .ForMember(des => des.CompRoleID, opt => opt.MapFrom(src => src.CompRoleId))
            .ForMember(des => des.CompRoleName, opt => opt.MapFrom(src => src.CompRoleName));

        CreateMap<SystemRoleDTO, SystemRole>()
            .ForMember(des => des.SysRoleId, opt => opt.MapFrom(src => src.SysRoleID))
            .ForMember(des => des.SysRoleName, opt => opt.MapFrom(src => src.SysRoleName));
        CreateMap<SystemRole, SystemRoleDTO>()
            .ForMember(des => des.SysRoleID, opt => opt.MapFrom(src => src.SysRoleId))
            .ForMember(des => des.SysRoleName, opt => opt.MapFrom(src => src.SysRoleName));
    }
}
