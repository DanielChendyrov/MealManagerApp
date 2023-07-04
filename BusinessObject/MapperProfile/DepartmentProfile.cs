using AutoMapper;
using BusinessObject.DTO;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDTO>()
            .ForMember(des => des.DepID, opt => opt.MapFrom(src => src.DepId))
            .ForMember(des => des.DepName, opt => opt.MapFrom(src => src.DepName));
        CreateMap<DepartmentDTO, Department>()
            .ForMember(des => des.DepId, opt => opt.MapFrom(src => src.DepID))
            .ForMember(des => des.DepName, opt => opt.MapFrom(src => src.DepName));
    }
}
