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

    public async Task<List<PersonalMonthlyStatsDTO>> GetPersonalMonthlyStats(int uid)
    {
        var dtoList = Mapper.Map<List<ServingDTO>>(await MealDAO.GetPersonalMonthlyStats(uid));
        var dateList = dtoList.Select(s => s.BookedDate.Date).Distinct().ToList();
        var mealList = dtoList.Select(s => s.Meal).DistinctBy(m => m.MealID).ToList();
        List<PersonalMonthlyStatsDTO> result = new();
        if (!dateList.IsNullOrEmpty() && !mealList.IsNullOrEmpty())
        {
            foreach (var d in dateList)
            {
                PersonalMonthlyStatsDTO item = new() { UserID = uid, BookedDate = d.Date, };
                foreach (var m in mealList)
                {
                    item.MealStats.Add(
                        new CustomMealStatsDTO
                        {
                            MealID = m.MealID,
                            Meal = m,
                            TotalServing = Convert.ToInt32(
                                dtoList
                                    .Where(s => s.BookedDate.Date == d.Date && s.MealID == m.MealID)
                                    .Select(s => s.Quantity)
                                    .FirstOrDefault()
                            ),
                        }
                    );
                }
                result.Add(item);
            }
        }
        return result;
    }

    public async Task<List<ServingDTO>> GetCompanyDailyStats(DateTime requestDate)
    {
        return Mapper.Map<List<ServingDTO>>(await MealDAO.GetCompanyDailyStats(requestDate));
    }

    public async Task<List<CompanyMonthlyStatsDTO>> GetCompanyMonthlyStats(DateTime requestDate)
    {
        var dtoList = Mapper.Map<List<ServingDTO>>(
            await MealDAO.GetCompanyMonthlyStats(requestDate)
        );
        var userList = dtoList.Select(s => s.User).DistinctBy(u => u.UserID).ToList();
        var mealList = dtoList.Select(s => s.Meal).DistinctBy(m => m.MealID).ToList();
        List<CompanyMonthlyStatsDTO> result = new();
        if (!userList.IsNullOrEmpty() && !mealList.IsNullOrEmpty())
        {
            foreach (var u in userList)
            {
                CompanyMonthlyStatsDTO item = new() { UserID = u!.UserID, User = u };
                foreach (var m in mealList)
                {
                    item.MealStats.Add(
                        new CustomMealStatsDTO
                        {
                            MealID = m!.MealID,
                            Meal = m,
                            TotalServing = dtoList
                                .Where(s => s.UserID == u.UserID && s.MealID == m.MealID)
                                .Select(s => s.Quantity)
                                .Sum(),
                        }
                    );
                }
                item.Total = item.MealStats.Select(ms => ms.TotalServing).Sum();
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
        request.Servings = request.Servings
            .Where(s => s.UserID == request.UserID && s.BookedDate >= DateTime.Now)
            .ToList();
        if (!request.Servings.IsNullOrEmpty())
        {
            return await MealDAO.RegisterMeal(Mapper.Map<Form>(request));
        }
        return false;
    }

    public async Task<bool> RegisterDepartmentMeal(FormDTO request)
    {
        request.Servings = request.Servings.Where(s => s.BookedDate >= DateTime.Now).ToList();
        if (!request.Servings.IsNullOrEmpty())
        {
            return await MealDAO.RegisterMeal(Mapper.Map<Form>(request));
        }
        return false;
    }

    public async Task<List<ServingDTO>> GetAllPersonalOrders(int uid)
    {
        return Mapper.Map<List<ServingDTO>>(await MealDAO.GetAllPersonalOrders(uid));
    }

    public async Task<bool> EditMeal(List<ServingDTO> request)
    {
        return await MealDAO.EditMeal(Mapper.Map<List<Serving>>(request));
    }

    public async Task<bool> DeleteMeal(int servingID)
    {
        return await MealDAO.DeleteMeal(servingID);
    }
}
