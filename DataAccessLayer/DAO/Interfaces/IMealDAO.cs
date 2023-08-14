using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IMealDAO
{
    Task<List<Serving>> GetPersonalMonthlyStats(int uid);
    Task<List<Serving>> GetCompanyMonthlyStats(DateTime requestDate);
    Task<List<Serving>> GetCompanyDailyStats(DateTime requestDate);
    Task<List<Serving>> FindExistingRegistration(int depID);
    Task<bool> RegisterMeal(Form request);
    Task<List<Meal>> GetAllMeals();
    Task<List<Serving>> GetAllPersonalOrders(int uid);
    Task<bool> EditMeal(List<Serving> request);
    Task<bool> DeleteMeal(int servingID);
    Task<bool> EditMeal3rdShift(List<Serving> request);
    Task<List<Serving>> GetAll3rdShiftMeals(DateTime bookedDate, int depID);
}
