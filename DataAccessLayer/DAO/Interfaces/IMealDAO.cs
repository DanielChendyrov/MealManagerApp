using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IMealDAO
{
    Task<List<Serving>> GetPersonalMonthlyStats(int uid);
    Task<List<Serving>> GetCompanyDailyStats(DateTime bookedDate);
    Task<List<Serving>> GetCompanyMonthlyStats(DateTime requestDate);
    Task<List<Serving>> FindExistingRegistration(int depID);
    Task<bool> RegisterMeal(Form request);
    Task<List<Meal>> GetAllMeals();
}
