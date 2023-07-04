using DataAccessLayer.Domain;

namespace DataAccessLayer.DAO.Interfaces;

public interface IMealDAO
{
    Task<List<Serving>> GetPersonalMonthlyStats(int uid);
    Task<List<Serving>> GetDepartmentDailyStats(DateTime bookedDate);
}
