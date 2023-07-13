using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.DTO.Response;
using BusinessObject.Manager.Interfaces;
using DataAccessLayer.DAO.Interfaces;
using DataAccessLayer.Domain;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<List<MealDTO>> GettAllMeals()
    {
        return Mapper.Map<List<MealDTO>>(await MealDAO.GetAllMeals());
    }

    public async Task<List<ServingDTO>> GetPersonalMonthlyStats(int uid)
    {
        return Mapper.Map<List<ServingDTO>>(await MealDAO.GetPersonalMonthlyStats(uid));
    }

    public async Task<List<ServingDTO>> GetCompanyDailyStats(DateTime requestDate)
    {
        return Mapper.Map<List<ServingDTO>>(await MealDAO.GetCompanyDailyStats(requestDate));
    }

    public async Task<List<CompanyMonthlyStatsDTO>> GetCompanyMonthlyStats(DateTime requestDate)
    {
        var response = await MealDAO.GetCompanyMonthlyStats(requestDate);
        var dtoList = Mapper.Map<List<ServingDTO>>(response);
        var userList = dtoList.Select(s => s.User).Distinct().ToList();
        var mealList = dtoList.Select(s => s.Meal).Distinct().ToList();
        List<CompanyMonthlyStatsDTO> result = new();
        if (!userList.IsNullOrEmpty() && !mealList.IsNullOrEmpty())
        {
            foreach (var u in userList)
            {
                CompanyMonthlyStatsDTO item = new() { UserID = u.UserID, User = u };
                foreach (var m in mealList)
                {
                    item.MealStats.Add(
                        new CustomMealStatsDTO
                        {
                            MealID = m.MealID,
                            Meal = m,
                            TotalServing = dtoList
                                .Where(s => s.UserID == u.UserID && s.MealID == m.MealID)
                                .Select(s => s.Quantity)
                                .Sum(),
                        }
                    );
                }
                result.Add(item);
            }
        }
        return result;
    }

    public async Task<List<ServingDTO>> FindExistingRegistration(int depID)
    {
        return Mapper.Map<List<ServingDTO>>(await MealDAO.FindExistingRegistration(depID));
    }

    public async Task<bool> RegisterPersonalMeal(FormDTO request)
    {
        if (!request.Servings.IsNullOrEmpty())
        {
            request.Servings = request.Servings
                .Where(s => s.UserID == request.UserID && s.BookedDate >= System.DateTime.Now)
                .ToList();
        }
        return await MealDAO.RegisterMeal(Mapper.Map<Form>(request));
    }

    public async Task<bool> RegisterDepartmentMeal(FormDTO request)
    {
        if (!request.Servings.IsNullOrEmpty())
        {
            request.Servings = request.Servings
                .Where(s => s.BookedDate >= System.DateTime.Now)
                .ToList();
        }
        return await MealDAO.RegisterMeal(Mapper.Map<Form>(request));
    }
}
