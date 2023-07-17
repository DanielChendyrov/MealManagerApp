using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class MealProfile : Profile
{
    public MealProfile()
    {
        CreateMap<FormDTO, Form>()
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserID))
            .ForMember(des => des.User, opt => opt.MapFrom(src => src.User))
            .ForMember(des => des.DepId, opt => opt.MapFrom(src => src.DepID))
            .ForMember(des => des.Dep, opt => opt.MapFrom(src => src.Dep))
            .ForMember(des => des.Servings, opt => opt.MapFrom(src => src.Servings));

        CreateMap<ServingDTO, Serving>()
            .ForMember(des => des.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(des => des.BookedDate, opt => opt.MapFrom(src => src.BookedDate))
            .ForMember(des => des.MealId, opt => opt.MapFrom(src => src.MealID))
            .ForMember(des => des.Meal, opt => opt.MapFrom(src => src.Meal))
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.UserID))
            .ForMember(des => des.User, opt => opt.MapFrom(src => src.User));
        CreateMap<Serving, ServingDTO>()
            .ForMember(des => des.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(des => des.BookedDate, opt => opt.MapFrom(src => src.BookedDate))
            .ForMember(des => des.MealID, opt => opt.MapFrom(src => src.MealId))
            .ForMember(des => des.Meal, opt => opt.MapFrom(src => src.Meal))
            .ForMember(des => des.UserID, opt => opt.MapFrom(src => src.UserId))
            .ForMember(des => des.User, opt => opt.MapFrom(src => src.User));
        ;

        CreateMap<Meal, MealDTO>()
            .ForMember(des => des.MealID, opt => opt.MapFrom(src => src.MealId))
            .ForMember(des => des.MealName, opt => opt.MapFrom(src => src.MealName));
        CreateMap<MealDTO, Meal>()
            .ForMember(des => des.MealId, opt => opt.MapFrom(src => src.MealID))
            .ForMember(des => des.MealName, opt => opt.MapFrom(src => src.MealName));
    }
}
