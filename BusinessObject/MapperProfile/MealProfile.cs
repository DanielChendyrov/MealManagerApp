using AutoMapper;
using BusinessObject.DTO.Request;
using DataAccessLayer.Domain;

namespace BusinessObject.MapperProfile;

public class MealProfile : Profile
{
    public MealProfile()
    {
        CreateMap<FormDTO, Form>()
            .ForMember(des => des.User.UserId, opt => opt.MapFrom(src => src.UserID))
            .ForMember(des => des.Dep.DepId, opt => opt.MapFrom(src => src.DepID))
            .ForMember(des => des.RegisteredDate, opt => opt.MapFrom(src => src.RegisteredDate))
            .ForMember(des => des.Servings, opt => opt.MapFrom(src => src.Servings));

        CreateMap<ServingDTO, Serving>()
            .ForMember(des => des.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(des => des.BookedDate, opt => opt.MapFrom(src => src.BookedDate))
            .ForMember(des => des.Meal.MealId, opt => opt.MapFrom(src => src.MealID))
            .ForMember(des => des.User.UserId, opt => opt.MapFrom(src => src.UserID));
    }
}
