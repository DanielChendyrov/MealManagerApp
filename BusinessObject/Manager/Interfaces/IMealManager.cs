using BusinessObject.DTO;
using BusinessObject.DTO.Request;
using BusinessObject.DTO.Response;

namespace BusinessObject.Manager.Interfaces;

public interface IMealManager
{
    Task<List<PersonalMonthlyStatsDTO>> GetPersonalMonthlyStats(int uid);
    Task<List<ServingDTO>> GetCompanyDailyStats(DateTime bookedDate);
    Task<List<CompanyMonthlyStatsDTO>> GetCompanyMonthlyStats(DateTime requestDate);
    Task<List<ServingDTO>> FindExistingRegistration(int depID);
    Task<bool> RegisterPersonalMeal(FormDTO request);
    Task<bool> RegisterDepartmentMeal(FormDTO request);
    Task<List<MealDTO>> GettAllMeals();
    Task<List<ServingDTO>> GetAllPersonalOrders(int uid);
    Task<bool> EditMeal(List<ServingDTO> request);
    Task<bool> DeleteMeal(int servingID);
    Task<bool> EditMeal3rdShift(List<ServingDTO> request);
}
