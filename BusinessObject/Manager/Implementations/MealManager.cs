using AutoMapper;
using BusinessObject.DTO.Request;
using BusinessObject.Manager.Interfaces;
using DataAccessLayer.DAO.Interfaces;

namespace BusinessObject.Manager.Implementations;

public class MealManager : IMealManager
{
    private IMealDAO MealDAO { get; }
    private IMapper Mapper { get; }

    public MealManager(IMealDAO mealDAO, IMapper mapper)
    {
        MealDAO = mealDAO;
        Mapper = mapper;
    }

    public async Task<List<ServingDTO>> GetPersonalMonthlyStats(int uid)
    {
        var response = await MealDAO.GetPersonalMonthlyStats(uid);
        return Mapper.Map<List<ServingDTO>>(response);
    }

    public async Task<List<ServingDTO>> GetDepartmentDailyStats(DateTime bookedDate)
    {
        var response = await MealDAO.GetDepartmentDailyStats(bookedDate);
        return Mapper.Map<List<ServingDTO>>(response);
    }
}
